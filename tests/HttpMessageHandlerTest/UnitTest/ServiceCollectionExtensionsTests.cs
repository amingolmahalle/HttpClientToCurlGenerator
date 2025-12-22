using System.Reflection;
using FluentAssertions;
using HttpClientToCurl.Config;
using HttpClientToCurl.Extensions;
using HttpClientToCurl.HttpMessageHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace HttpMessageHandlerTest.UnitTest;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ServiceCollectionExtensions_Register_Services_For_All()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .Build();

        // Act
        services.AddAllHttpClientToCurl(configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var filter = provider.GetService<IHttpMessageHandlerBuilderFilter>();
        filter.Should().NotBeNull();
        filter.Should().BeOfType<HttpMessageHandlerAppender>();

        var handler = provider.GetService<CurlGeneratorHttpMessageHandler>();
        handler.Should().NotBeNull();

        var options = provider.GetService<IOptions<CompositConfig>>();
        options.Should().NotBeNull();
    }

    [Fact]
    public void ServiceCollectionExtensions_Register_Services_In_SpecificMode()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .Build();

        // Act
        services.AddHttpClientToCurl(configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var filter = provider.GetService<IHttpMessageHandlerBuilderFilter>();
        filter.Should().BeNull();

        var handler = provider.GetService<CurlGeneratorHttpMessageHandler>();
        handler.Should().NotBeNull();

        var options = provider.GetService<IOptions<CompositConfig>>();
        options.Should().NotBeNull();
    }

    [Fact]
    public void ServiceCollectionExtensions_AddHttpMessageHandler_When_ShowCurl_IsEnabled()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddTransient<CurlGeneratorHttpMessageHandler>();
        services.AddHttpClient("TestClient").AddCurlLogging();

        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act
        var httpClient = httpClientFactory.CreateClient("TestClient");

        // Assert
        var hasHandler = HasCurlGeneratorHttpMessageHandler(httpClient);
        hasHandler.Should().BeTrue();
    }

    [Fact]
    public void ServiceCollectionExtensions_AddHttpMessageHandler_When_ShowCurl_IsNotEnabled()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddTransient<CurlGeneratorHttpMessageHandler>();
        services.AddHttpClient("TestClient");

        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act
        var httpClient = httpClientFactory.CreateClient("TestClient");

        // Assert
        var hasHandler = HasCurlGeneratorHttpMessageHandler(httpClient);
        hasHandler.Should().BeFalse();
    }

    private static bool HasCurlGeneratorHttpMessageHandler(HttpClient client)
    {
        var field = typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance);
        var handler = field?.GetValue(client);

        while (handler is DelegatingHandler delegatingHandler)
        {
            if (delegatingHandler.GetType() == typeof(CurlGeneratorHttpMessageHandler))
            {
                return true;
            }
            handler = delegatingHandler.InnerHandler;
        }

        return false;
    }
}
