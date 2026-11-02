using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Concrete.Common;

/// <summary>
/// Builds the curl command for one HTTP method. The per-method builders only differ in how they treat the body,
/// so the whole pipeline lives here.
/// </summary>
public abstract class RequestBuilder : BaseBuilder
{
    private readonly BodyMode _bodyMode;

    private protected RequestBuilder(BodyMode bodyMode)
    {
        _bodyMode = bodyMode;
    }

    public sealed override string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        _stringBuilder
            .Initialize(httpRequestMessage.Method)
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(
                httpRequestMessage,
                config?.RedactedHeaders,
                httpClient,
                config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders);

        return AddBodyAndFinalize(httpRequestMessage, config);
    }

    public sealed override string CreateCurl(HttpRequestMessage httpRequestMessage, Uri baseAddress, BaseConfig config)
    {
        _stringBuilder
            .Initialize(httpRequestMessage.Method)
            .AddAbsoluteUrl(baseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpRequestMessage, config?.RedactedHeaders);

        return AddBodyAndFinalize(httpRequestMessage, config);
    }

    private string AddBodyAndFinalize(HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        // curl refuses --head combined with a body ("You can only select one HTTP request method").
        bool addBody = httpRequestMessage.Method != HttpMethod.Head && _bodyMode switch
        {
            BodyMode.None => false,
            BodyMode.Always => true,
            BodyMode.WhenContentPresent => httpRequestMessage.Content is not null,
            _ => false,
        };

        if (addBody)
        {
            _stringBuilder.AddBody(httpRequestMessage.Content);
        }

        return _stringBuilder.Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    private protected enum BodyMode
    {
        /// <summary>Never emit a body (GET, DELETE).</summary>
        None,

        /// <summary>Always emit a body argument, even for empty content (POST, PUT, PATCH).</summary>
        Always,

        /// <summary>Emit a body only when the request has content (HEAD, OPTIONS and any other method).</summary>
        WhenContentPresent,
    }
}
