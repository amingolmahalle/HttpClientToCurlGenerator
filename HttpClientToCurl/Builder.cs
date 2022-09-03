using System.Text;

namespace HttpClientToCurl;

internal static class Builder
{
    #region :: Builders ::

    internal static StringBuilder Initialize(HttpMethod httpMethod)
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

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string baseUrl, string uri)
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

    internal static StringBuilder AddHeaders(
        this StringBuilder stringBuilder,
        HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        bool needAddDefaultHeaders = true)
    {
        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
            foreach (var row in httpClient.DefaultRequestHeaders)
            {
                stringBuilder
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

    internal static StringBuilder AddBody(this StringBuilder stringBuilder, string jsonBody)
    {
        if (jsonBody != null && jsonBody.Any())
        {
            stringBuilder
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