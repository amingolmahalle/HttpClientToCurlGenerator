using System.Net;
using FluentAssertions;
using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Builders;
using HttpMessageHandlerTest.UnitTest.Fakes;
using Microsoft.Extensions.Logging;

namespace HttpMessageHandlerTest.UnitTest;

[Collection(ConsoleCollection.Name)]
public class CurlGeneratorHttpMessageHandlerResilienceAndRedactionTests
{
    private static async Task<HttpResponseMessage> SendAsync(
        CompositConfig config, HttpRequestMessage request, FakeLogger<CurlGeneratorHttpMessageHandler>? logger = null)
    {
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config), logger)
        {
            InnerHandler = new FakeHttpMessageHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);
        return await invoker.SendAsync(request, CancellationToken.None);
    }

    [Fact]
    public async Task Handler_Redacts_Headers_For_Logger_Sink()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var config = new CompositConfigBuilder()
            .SetSendToLogger(new LoggerConfig { RedactedHeaders = ["Authorization"] })
            .Build();
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
        request.Headers.Add("Authorization", "Bearer super-secret");

        await SendAsync(config, request, logger);

        var message = logger.LoggedMessages.Should().ContainSingle().Subject.Message;
        message.Should().Contain("Authorization: [REDACTED]");
        message.Should().NotContain("super-secret");
    }

    [Fact]
    public async Task Handler_Redacts_Headers_For_Console_Sink()
    {
        var config = new CompositConfigBuilder()
            .SetShowOnConsole(new ConsoleConfig { RedactedHeaders = ["Authorization"] })
            .Build();
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
        request.Headers.Add("Authorization", "Bearer super-secret");

        var original = Console.Out;
        var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            await SendAsync(config, request);
        }
        finally
        {
            Console.SetOut(original);
        }

        writer.ToString().Should().Contain("Authorization: [REDACTED]").And.NotContain("super-secret");
    }

    [Fact]
    public async Task Handler_Redacts_Headers_For_File_Sink()
    {
        var dir = Directory.CreateTempSubdirectory().FullName;
        try
        {
            var config = new CompositConfigBuilder()
                .SetSaveToFile(new FileConfig { RedactedHeaders = ["Authorization"], Path = dir, Filename = "out" })
                .Build();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
            request.Headers.Add("Authorization", "Bearer super-secret");

            await SendAsync(config, request);

            var content = await File.ReadAllTextAsync(Path.Combine(dir, "out.curl"));
            content.Should().Contain("Authorization: [REDACTED]").And.NotContain("super-secret");
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Theory]
    [InlineData("HEAD")]
    [InlineData("OPTIONS")]
    public async Task Handler_Still_Sends_Request_When_Curl_Generation_Fails(string method)
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var config = new CompositConfigBuilder()
            .SetShowOnConsole(new ConsoleConfig())
            .SetSendToLogger(new LoggerConfig())
            .Build();
        var request = new HttpRequestMessage(new HttpMethod(method), "http://localhost/test");

        var response = await SendAsync(config, request, logger);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        logger.LoggedMessages.Should().Contain(m => m.Level == LogLevel.Warning);
    }
}

[Collection(ConsoleCollection.Name)]
public class CurlGeneratorHttpMessageHandlerSinkBehaviourTests
{
    private static async Task<HttpResponseMessage> SendAsync(
        CompositConfig config, HttpRequestMessage request, FakeLogger<CurlGeneratorHttpMessageHandler>? logger = null)
    {
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config), logger)
        {
            InnerHandler = new FakeHttpMessageHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);
        return await invoker.SendAsync(request, CancellationToken.None);
    }

    [Fact]
    public async Task Failure_Without_Logger_Does_Not_Throw_And_Request_Is_Sent()
    {
        var config = new CompositConfigBuilder()
            .SetShowOnConsole(new ConsoleConfig())
            .Build();

        var original = Console.Out;
        Console.SetOut(new StringWriter());
        try
        {
            var response = await SendAsync(config, new HttpRequestMessage(HttpMethod.Head, "http://localhost/test"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    [Fact]
    public async Task Failing_File_Sink_Does_Not_Block_Logger_Sink_Or_The_Request()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var missingDirectory = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid():N}");
        var config = new CompositConfigBuilder()
            .SetSaveToFile(new FileConfig { Path = missingDirectory, Filename = "out" })
            .SetSendToLogger(new LoggerConfig { LogLevel = LogLevel.Information })
            .Build();

        var response = await SendAsync(config, new HttpRequestMessage(HttpMethod.Get, "http://localhost/test"), logger);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        logger.LoggedMessages.Should().Contain(m => m.Level == LogLevel.Warning);
        logger.LoggedMessages.Should().Contain(m => m.Level == LogLevel.Information && m.Message.StartsWith("curl"));
    }

    [Fact]
    public async Task Redaction_Matches_Header_Names_Case_Insensitively()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var config = new CompositConfigBuilder()
            .SetSendToLogger(new LoggerConfig { RedactedHeaders = ["aUtHoRiZaTiOn"] })
            .Build();
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
        request.Headers.Add("Authorization", "Bearer super-secret");

        await SendAsync(config, request, logger);

        logger.LoggedMessages.Single().Message.Should().Contain("Authorization: [REDACTED]").And.NotContain("super-secret");
    }

    [Fact]
    public async Task EnableCompression_Is_Copied_To_Logger_Sink()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var config = new CompositConfigBuilder()
            .SetSendToLogger(new LoggerConfig { EnableCompression = true })
            .Build();

        await SendAsync(config, new HttpRequestMessage(HttpMethod.Get, "http://localhost/test"), logger);

        logger.LoggedMessages.Single().Message.Should().EndWith("--compressed");
    }

    [Fact]
    public async Task EnableCompression_Is_Not_Added_By_Default()
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var config = new CompositConfigBuilder()
            .SetSendToLogger(new LoggerConfig())
            .Build();

        await SendAsync(config, new HttpRequestMessage(HttpMethod.Get, "http://localhost/test"), logger);

        logger.LoggedMessages.Single().Message.Should().NotContain("--compressed");
    }
}
