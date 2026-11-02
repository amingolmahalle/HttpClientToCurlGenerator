using System.Net;
using System.Text;
using FluentAssertions;
using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Builders;
using HttpMessageHandlerTest.UnitTest.Fakes;

namespace HttpMessageHandlerTest.UnitTest;

public class BodyBufferingTests
{
    private sealed class CapturingHandler : HttpMessageHandler
    {
        public string? ReceivedBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ReceivedBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    private sealed class CountingContent(string mediaType) : HttpContent
    {
        public int SerializeCalls { get; private set; }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            SerializeCalls++;
            return stream.WriteAsync(new byte[] { 1, 2, 3 }).AsTask();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 3;
            return true;
        }

        public CountingContent Typed()
        {
            Headers.ContentType = new(mediaType);
            return this;
        }
    }

    private sealed class NonSeekableStream(byte[] data) : MemoryStream(data)
    {
        public override bool CanSeek => false;
    }

    /// <summary>Content that completes asynchronously and captures the ambient SynchronizationContext, like most app code does.</summary>
    private sealed class ContextCapturingContent(string text) : HttpContent
    {
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            await Task.Delay(20); // continuation is posted back to the captured context
            var bytes = Encoding.UTF8.GetBytes(text);
            await stream.WriteAsync(bytes);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }

    private sealed class SingleThreadContext : SynchronizationContext, IDisposable
    {
        private readonly System.Collections.Concurrent.BlockingCollection<(SendOrPostCallback, object?)> _queue = [];
        private readonly Thread _thread;

        public SingleThreadContext()
        {
            _thread = new Thread(() =>
            {
                SetSynchronizationContext(this);
                foreach (var (callback, state) in _queue.GetConsumingEnumerable())
                {
                    callback(state);
                }
            })
            { IsBackground = true };
            _thread.Start();
        }

        public override void Post(SendOrPostCallback d, object? state) => _queue.Add((d, state));

        public Task<T> RunAsync<T>(Func<Task<T>> work)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            Post(async _ =>
            {
                try { tcs.SetResult(await work()); }
                catch (Exception ex) { tcs.SetException(ex); }
            }, null);
            return tcs.Task;
        }

        public void Dispose() => _queue.CompleteAdding();
    }

    private static CompositConfig LoggerConfig() => new CompositConfigBuilder()
        .SetSendToLogger(new LoggerConfig())
        .Build();

    [Fact]
    public async Task Body_Is_Still_Delivered_Intact_When_Content_Is_A_NonSeekable_Stream()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var inner = new CapturingHandler();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(LoggerConfig()), logger)
        {
            InnerHandler = inner
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/test")
        {
            Content = new StreamContent(new NonSeekableStream(Encoding.UTF8.GetBytes("{\"a\":1}")))
        };
        request.Content.Headers.ContentType = new("application/json");

        using var invoker = new HttpMessageInvoker(handler);
        var response = await invoker.SendAsync(request, CancellationToken.None);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        inner.ReceivedBody.Should().Be("{\"a\":1}");
        logger.LoggedMessages.Single().Message.Should().Contain("-d '{\"a\":1}'");
    }

    [Fact]
    public async Task Handler_Does_Not_Block_The_Calling_Thread_While_Reading_The_Body()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(LoggerConfig()), logger)
        {
            InnerHandler = new CapturingHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);
        using var context = new SingleThreadContext();

        var work = context.RunAsync(async () =>
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/test")
            {
                Content = new ContextCapturingContent("hello")
            };
            var response = await invoker.SendAsync(request, CancellationToken.None);
            return response.StatusCode;
        });

        var finished = await Task.WhenAny(work, Task.Delay(TimeSpan.FromSeconds(5)));

        finished.Should().BeSameAs(work, "blocking on the body read deadlocks under a single-threaded SynchronizationContext");
        (await work).Should().Be(HttpStatusCode.OK);
        logger.LoggedMessages.Single().Message.Should().Contain("-d 'hello'");
    }

    [Fact]
    public async Task Binary_Body_Is_Not_Read_Just_To_Print_A_Placeholder()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(LoggerConfig()), logger)
        {
            InnerHandler = new FakeHttpMessageHandler()
        };
        var content = new CountingContent("application/octet-stream").Typed();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/test") { Content = content };

        using var invoker = new HttpMessageInvoker(handler);
        await invoker.SendAsync(request, CancellationToken.None);

        content.SerializeCalls.Should().Be(0);
        logger.LoggedMessages.Single().Message.Should().Contain("--data-binary '@payload.bin'");
    }

    [Fact]
    public async Task Multipart_File_Part_Is_Not_Read_Just_To_Print_A_Placeholder()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(LoggerConfig()), logger)
        {
            InnerHandler = new FakeHttpMessageHandler()
        };
        var filePart = new CountingContent("image/png").Typed();
        var multipart = new MultipartFormDataContent("b") { { new StringContent("hello"), "note" }, { filePart, "avatar", "pic.png" } };
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/test") { Content = multipart };

        using var invoker = new HttpMessageInvoker(handler);
        await invoker.SendAsync(request, CancellationToken.None);

        filePart.SerializeCalls.Should().Be(0);
        var message = logger.LoggedMessages.Single().Message;
        message.Should().Contain("--form-string 'note=hello'").And.Contain("-F 'avatar=@pic.png;type=image/png'");
    }
}
