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
                script = _GeneratePostMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            else if (httpRequestMessage.Method == HttpMethod.Get)
                script = _GenerateGetMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            else if (httpRequestMessage.Method == HttpMethod.Put)
                script = _GeneratePutMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            else if (httpRequestMessage.Method == HttpMethod.Patch)
                script = _GeneratePatchMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            else if (httpRequestMessage.Method == HttpMethod.Delete)
                script = _GenerateDeleteMethod(httpClient, httpRequestMessage, needAddDefaultHeaders);
            else
                throw new DataException($"invalid HttpMethod: {httpRequestMessage.Method.Method}!");
        }
        catch (Exception exception)
        {
            script = $"GenerateCurlError => {exception.Message}.{exception.InnerException}";
        }

        return script;
    }

    #region :: CURL GENERATORS ::

    private static string _GenerateGetMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri?.ToString())
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .Append(' ')
            .ToString();
    }

    private static string _GeneratePostMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri?.ToString())
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string _GeneratePutMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri?.ToString())
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string _GeneratePatchMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri?.ToString())
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .AddBody(httpRequestMessage.Content)?
            .Append(' ')
            .ToString();
    }

    private static string _GenerateDeleteMethod(HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders)
    {
        StringBuilder stringBuilder = Builder.Initialize(httpRequestMessage.Method);

        return stringBuilder
            .AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, httpRequestMessage.RequestUri?.ToString())
            .AddHeaders(httpClient, httpRequestMessage, needAddDefaultHeaders)?
            .Append(' ')
            .ToString();
    }

    #endregion
}