using System.Data;
using System.Text;

namespace HttpClientToCurl;

public static class Generator
{
    public static string GenerateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        string script;

        try
        {
            if (httpRequestMessage.Method == HttpMethod.Post)
            {
                script = GeneratePostMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            }
            else if (httpRequestMessage.Method == HttpMethod.Get)
            {
                script = GenerateGetMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            }
            else if (httpRequestMessage.Method == HttpMethod.Put)
            {
                script = GeneratePutMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            }
            else if (httpRequestMessage.Method == HttpMethod.Patch)
            {
                script = GeneratePatchMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            }
            else if (httpRequestMessage.Method == HttpMethod.Delete)
            {
                script = GenerateDeleteMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
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

    private static string GenerateGetMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .Append(' ')
            .ToString();
    }

    private static string GeneratePostMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string GeneratePutMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string GeneratePatchMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string GenerateDeleteMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri)
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .Append(' ')
            .ToString();
    }

    #endregion :: CURL GENERATORS ::
}
