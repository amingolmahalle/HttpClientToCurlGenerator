using System.Data;
using System.Text;
using HttpClientToCurl.Config;

namespace HttpClientToCurl;

public static class Generator
{
    public static string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        string script;

        try
        {
            if (httpRequestMessage.Method == HttpMethod.Post)
            {
                script = GenerateForPostMethod(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Get)
            {
                script = GenerateForGetMethod(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Put)
            {
                script = GenerateForPutMethod(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Patch)
            {
                script = GenerateForPatchMethod(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Delete)
            {
                script = GenerateForDeleteMethod(httpClient, httpRequestMessage, config);
            }
            else
            {
                throw new DataException($"not supported {httpRequestMessage.Method.Method}!");
            }
        }
        catch (Exception exception)
        {
            script = $"GenerateCurlError => {exception.Message} {exception.InnerException}";
        }

        return script;
    }

    #region :: CURL GENERATORS ::

    private static string GenerateForGetMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    private static string GenerateForPostMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    private static string GenerateForPutMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    private static string GenerateForPatchMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    private static string GenerateForDeleteMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, config?.NeedAddDefaultHeaders ?? new BaseConfig().NeedAddDefaultHeaders)?
            .Finalize(config?.EnableCompression ?? new BaseConfig().EnableCompression);
    }

    #endregion :: CURL GENERATORS ::
}
