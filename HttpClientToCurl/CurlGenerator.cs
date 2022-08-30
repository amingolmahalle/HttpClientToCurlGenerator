using System.Text;

namespace HttpClientToCurl;

public static class CurlGenerator
{
    #region :: Main ::

    public static string GenerateCurl(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri,
        bool turnOn = true)
    {
        if (!turnOn)
            return string.Empty;

        string script;
        try
        {
            if (httpRequestMessage.Method == HttpMethod.Post)
                script = _GeneratePostMethod(httpClient, httpRequestMessage, requestUri);
            else if (httpRequestMessage.Method == HttpMethod.Get)
                script = _GenerateGetMethod(httpClient, httpRequestMessage, requestUri);
            else if (httpRequestMessage.Method == HttpMethod.Put)
                script = _GeneratePutMethod(httpClient, httpRequestMessage, requestUri);
            else if (httpRequestMessage.Method == HttpMethod.Patch)
                script = _GeneratePatchMethod(httpClient, httpRequestMessage, requestUri);
            else if (httpRequestMessage.Method == HttpMethod.Delete)
                script = _GenerateDeleteMethod(httpClient, httpRequestMessage, requestUri);
            else
                script = $"ERROR => Invalid HttpMethod: {httpRequestMessage.Method.Method} .";
        }
        catch (Exception exception)
        {
            script = $"ERROR => {exception.Message}, {exception.InnerException}";
        }

        return script;
    }

    #endregion

    #region :: Generators ::

    private static string _GenerateGetMethod(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri)
    {
        StringBuilder stringBuilder = _Initialize(httpRequestMessage.Method);

        return stringBuilder
            ._AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, requestUri)
            ._AddHeaders(httpClient, httpRequestMessage, true)
            .Append(' ')
            .ToString();
    }

    private static string _GeneratePostMethod(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri)
    {
        StringBuilder stringBuilder = _Initialize(httpRequestMessage.Method);

        return stringBuilder
            ._AddAbsoluteUrl(httpClient.BaseAddress?.AbsoluteUri, requestUri)
            ._AddHeaders(httpClient, httpRequestMessage, true)
            ._AddBody(httpRequestMessage.Content?.ReadAsStringAsync().GetAwaiter().GetResult())?
            .Append(' ')
            .ToString();
    }

    private static string _GeneratePutMethod(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri)
    {
        return "";
    }

    private static string _GeneratePatchMethod(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri)
    {
        return "";
    }

    private static string _GenerateDeleteMethod(
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri)
    {
        return "";
    }

    #endregion

    #region :: Builders ::

    private static StringBuilder _Initialize(HttpMethod httpMethod)
    {
        var stringBuilder = new StringBuilder("curl");

        if (httpMethod != HttpMethod.Get)
        {
            stringBuilder
                .Append(' ')
                .Append("-X")
                .Append(' ')
                .Append(httpMethod.Method)
                .Append(' ');
        }

        return stringBuilder;
    }

    private static StringBuilder _AddAbsoluteUrl(this StringBuilder stringBuilder, string baseUrl, string uri)
    {
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            if (baseUrl.EndsWith("/"))
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);
            if (uri.StartsWith("/"))
                uri = uri.Remove(0);

            stringBuilder.Append($"{baseUrl?.Trim()}/{uri?.Trim()}");
        }

        return stringBuilder;
    }

    private static StringBuilder _AddHeaders(
        this StringBuilder stringBuilder,
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        bool needAddDefaultHeaders = true)
    {
        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
            foreach (var row in httpClient.DefaultRequestHeaders)
            {
                stringBuilder
                    .Append(' ')
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

        if (httpRequestMessage.Headers.Any())
        {
            foreach (var row in httpRequestMessage.Headers)
            {
                stringBuilder
                    .Append(' ')
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }
        }

        if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var row in httpRequestMessage.Content.Headers)
            {
                stringBuilder
                    .Append(' ')
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }
        }

        return stringBuilder;
    }

    private static StringBuilder _AddBody(this StringBuilder stringBuilder, string jsonBody)
    {
        if (jsonBody != null && jsonBody.Any())
        {
            stringBuilder
                .Append(' ')
                .Append("-d")
                .Append(' ')
                .Append('\'')
                .Append(jsonBody)
                .Append('\'')
                .Append(' ');
        }

        return stringBuilder;
    }

    #endregion
}