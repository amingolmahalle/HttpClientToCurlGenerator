using FluentAssertions;
using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Builders;
using HttpMessageHandlerTest.UnitTest.Fakes;

namespace HttpMessageHandlerTest.UnitTest;

public class HttpMessageHandlerAppenderTests
{
    [Fact]
    public void HttpMessageHandlerAppender_Adds_Handler_To_Builder()
    {
        // Arrange
        var config = new CompositConfig { Enable = false };
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config));
        var sp = new FakeServiceProvider(handler);
        var appender = new HttpMessageHandlerAppender(sp);

        var builder = new HttpMessageHandlerBuilder();

        // Act
        var configure = appender.Configure(next => { });
        configure(builder);

        // Assert
        builder.AdditionalHandlers.Should().ContainSingle()
            .Which.Should().Be(handler);
    }
}
