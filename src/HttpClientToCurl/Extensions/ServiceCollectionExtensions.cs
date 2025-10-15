using HttpClientToCurl.Config;
using HttpClientToCurl.HttpMessageHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace HttpClientToCurl.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Generating curl script of all HTTP requests.
    /// <para> By default, show it in the IDE console. </para>
    /// </summary>
    /// <param name="configAction">Optional</param>
    public static void AddHttpClientToCurl(
        this IServiceCollection services,
        Action<GlobalConfig> configAction = null)
    {
        var config = new GlobalConfig();
        configAction?.Invoke(config);

        services.AddSingleton(config);
        services.AddTransient<CurlGeneratorHttpMessageHandler>();
        services.Add(ServiceDescriptor.Transient<IHttpMessageHandlerBuilderFilter, HttpMessageHandlerAppender>());
    }
}
