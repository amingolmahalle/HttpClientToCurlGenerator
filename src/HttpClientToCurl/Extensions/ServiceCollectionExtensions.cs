using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace HttpClientToCurl.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Generating curl script for all HTTP requests.
    /// <para> By default, show it in the IDE console. </para>
    /// </summary>
    public static void AddAllHttpClientToCurl(
           this IServiceCollection services,
           IConfiguration configuration)
    {
        AddServices(services, configuration);
        services.Add(ServiceDescriptor.Transient<IHttpMessageHandlerBuilderFilter, HttpMessageHandlerAppender>());
    }

    /// <summary>
    /// Generating curl script for specific http client 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddHttpClientToCurl(
           this IServiceCollection services,
           IConfiguration configuration)
    {
        AddServices(services, configuration);
    }

    [Obsolete($@"
        {nameof(AddHttpClient)} extension method is deprecated and will be removed in future versions.
        Instead, please use {nameof(AddCurlLogging)}, which is an extension built on top of the original AddHttpClient."
    )]
    public static IHttpClientBuilder AddHttpClient(this IServiceCollection services, string name, bool showCurl = false)
    {
        var httpClientBuilder = HttpClientFactoryServiceCollectionExtensions.AddHttpClient(services, name);

        if (showCurl)
        {
            httpClientBuilder.AddHttpMessageHandler<CurlGeneratorHttpMessageHandler>();
        }

        return httpClientBuilder;
    }

    public static void AddCurlLogging(this IHttpClientBuilder httpClientBuilder)
        => httpClientBuilder.AddHttpMessageHandler<CurlGeneratorHttpMessageHandler>();

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CompositConfig>(configuration.GetSection("HttpClientToCurl"));
        services.AddTransient<CurlGeneratorHttpMessageHandler>();
    }
}
