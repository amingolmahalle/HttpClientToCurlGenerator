using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace HttpClientToCurl.HttpMessageHandlers;

public class HttpMessageHandlerAppender(IServiceProvider serviceProvider) : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next) => builder =>
    {
        next(builder);
        var handler = serviceProvider.GetRequiredService<CurlGeneratorHttpMessageHandler>();
        builder.AdditionalHandlers.Add(handler);
    };
}
