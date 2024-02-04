using HttpClientToCurl.Builder.Concrete.Common;
using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Concrete;

public sealed class HttpGetBuilder : BaseBuilder, Interface.IBuilder
{
    public string Create(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        return _stringBuilder
            .Initialize(httpRequestMessage.Method)
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }
}