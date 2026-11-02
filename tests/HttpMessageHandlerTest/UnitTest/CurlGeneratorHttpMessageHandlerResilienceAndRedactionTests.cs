using System.Net;
using FluentAssertions;
using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Builders;
using HttpMessageHandlerTest.UnitTest.Fakes;
using Microsoft.Extensions.Logging;

namespace HttpMessageHandlerTest.UnitTest;

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
