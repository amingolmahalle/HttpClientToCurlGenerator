using HttpClientToCurl.Config;
using HttpClientToCurl.Config.Others;
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
    /// <param name="isGlobal">Apply for all http requests in the application or not. Default is true.</param>
    public static void AddHttpClientToCurl(
    this IServiceCollection services,
    Action<GlobalConfig> configAction = null,
    bool isGlobal = true)
    {
        configAction ??= config => config.ShowMode = ShowMode.Console;

        var config = new GlobalConfig();
        configAction?.Invoke(config);

        services.AddSingleton(config);
        services.AddTransient<CurlGeneratorHttpMessageHandler>();

        if (isGlobal)
        {
            services.Add(ServiceDescriptor.Transient<IHttpMessageHandlerBuilderFilter, HttpMessageHandlerAppender>());
        }
    }
}
