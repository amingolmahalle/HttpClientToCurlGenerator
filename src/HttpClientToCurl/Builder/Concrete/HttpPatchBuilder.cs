using HttpClientToCurl.Builder.Concrete.Common;
using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Concrete;

public sealed class HttpPatchBuilder : BaseBuilder
{
    public override string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        return _stringBuilder
            .Initialize(httpRequestMessage.Method)
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    public override string CreateCurl(HttpRequestMessage httpRequestMessage, Uri baseAddress, BaseConfig config)
    {
        return _stringBuilder
           .Initialize(httpRequestMessage.Method)
           .AddAbsoluteUrl(baseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
           .AddHeaders(httpRequestMessage)?
           .AddBody(httpRequestMessage.Content)?
           .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }
}
