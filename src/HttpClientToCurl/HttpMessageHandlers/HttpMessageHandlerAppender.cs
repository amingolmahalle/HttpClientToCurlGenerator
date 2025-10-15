using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace HttpClientToCurl.HttpMessageHandlers;
public class HttpMessageHandlerAppender : IHttpMessageHandlerBuilderFilter
{
    private readonly IServiceProvider _serviceProvider;

    public HttpMessageHandlerAppender(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next) => builder =>
    {
        next(builder);
        var handler = _serviceProvider.GetRequiredService<CurlGeneratorHttpMessageHandler>();
        builder.AdditionalHandlers.Add(handler);
    };
}
