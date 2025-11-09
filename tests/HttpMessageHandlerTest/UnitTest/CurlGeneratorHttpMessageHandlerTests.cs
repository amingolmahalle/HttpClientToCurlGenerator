using System.Net;
using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Fakes;
using HttpMessageHandlerTest.UnitTest.Builders;
using FluentAssertions;

namespace HttpMessageHandlerTest.UnitTest;

public class CurlGeneratorHttpMessageHandlerTests
{
    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_ReturnsResponse_When_TurnOffAll()
    {
        // Arrange
        var config = new CompositConfigBuilder()
            .SetTurnOnAll(false)
            .Build();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config))
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");

        // Act
        using var invoker = new HttpMessageInvoker(handler);
        var response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_ReturnsResponse_When_TurnOnAll_But_ShowOnConsole_And_SaveToFile_AreNot_Configured()
    {
        // Arrange
        var config = new CompositConfigBuilder()
            .SetTurnOnAll(true)
            .SetShowOnConsole(null)
            .SetSaveToFile(null)
            .Build();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config))
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");

        // Act
        using var invoker = new HttpMessageInvoker(handler);
        var response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_ReturnsResponse_When_TurnOnAll_But_ShowOnConsole_And_SaveToFile_TurnOff()
    {
        // Arrange
        var config = new CompositConfigBuilder()
            .SetTurnOnAll(true)
            .SetShowOnConsole(new ConsoleConfig
            {
                TurnOn = false,
            })
            .SetSaveToFile(new FileConfig()
            {
                TurnOn = false
            })
            .Build();
        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config))
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");

        // Act
        using var invoker = new HttpMessageInvoker(handler);
        var response = await invoker.SendAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_WritesToConsole_When_ShowOnConsole_TurnOn()
    {
        // Arrange
        var config = new CompositConfigBuilder()
            .SetTurnOnAll(true)
            .SetShowOnConsole(new ConsoleConfig
            {
                TurnOn = true,
                EnableCodeBeautification = false
            })
            .Build();

        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config))
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");

        var sw = new StringWriter();
        var originalOut = Console.Out;
        try
        {
            Console.SetOut(sw);

            // Act
            using var invoker = new HttpMessageInvoker(handler);
            var response = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var output = sw.ToString();
            output.Should().Contain("curl");
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_WritesToFile_When_SaveToFile_TurnOn()
    {
        // Arrange
        var tempPath = Path.GetTempPath();
        var filename = Guid.NewGuid().ToString("N");

        var config = new CompositConfigBuilder()
            .SetTurnOnAll(true)
            .SetSaveToFile(new FileConfig
            {
                TurnOn = true,
                Path = tempPath,
                Filename = filename
            })
            .Build();

        var monitor = new FakeOptionsMonitor<CompositConfig>(config);
        var handler = new CurlGeneratorHttpMessageHandler(monitor)
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/test") { Content = new StringContent("hello") };

        var filePath = Path.Combine(tempPath.TrimEnd(Path.DirectorySeparatorChar), filename + ".curl");

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Act
            using var invoker = new HttpMessageInvoker(handler);
            var response = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            File.Exists(filePath).Should().BeTrue();
            var content = File.ReadAllText(filePath);
            content.Should().Contain("curl");
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public async Task CurlGeneratorHttpMessageHandler_WritesToConsole_And_WritesToFile_When_ShowOnConsole_And_SaveToFile_TurnOn()
    {
        // Arrange
        var tempPath = Path.GetTempPath();
        var filename = Guid.NewGuid().ToString("N");

        var config = new CompositConfigBuilder()
            .SetTurnOnAll(true)
            .SetShowOnConsole(new ConsoleConfig
            {
                TurnOn = true,
                EnableCodeBeautification = false
            })
            .SetSaveToFile(new FileConfig
            {
                TurnOn = true,
                Path = tempPath,
                Filename = filename
            })
            .Build();

        var handler = new CurlGeneratorHttpMessageHandler(new FakeOptionsMonitor<CompositConfig>(config))
        {
            InnerHandler = new FakeHttpMessageHandler()
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");

        var sw = new StringWriter();
        var originalOut = Console.Out;
        var filePath = Path.Combine(tempPath.TrimEnd(Path.DirectorySeparatorChar), filename + ".curl");

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            Console.SetOut(sw);

            // Act
            using var invoker = new HttpMessageInvoker(handler);
            var response = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var output = sw.ToString();
            output.Should().Contain("curl");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            File.Exists(filePath).Should().BeTrue();
            var content = File.ReadAllText(filePath);
            content.Should().Contain("curl");
        }
        finally
        {
            Console.SetOut(originalOut);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
