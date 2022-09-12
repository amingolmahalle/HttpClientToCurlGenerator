using System.Net.Mime;
using System.Text;
using System.Web;
using HttpClientToCurl.Utility;
using Newtonsoft.Json;

namespace HttpClientToCurl;

internal static class Builder
{
    internal static StringBuilder Initialize(HttpMethod httpMethod)
    {
        var stringBuilder = new StringBuilder("curl");

        if (httpMethod != HttpMethod.Get)
        {
            stringBuilder
                .Append(' ')
                .Append("-X")
                .Append(' ')
                .Append(httpMethod.Method);
        }

        return stringBuilder
            .Append(' ');
    }

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string baseUrl, string uri)
    {
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            string inputBaseUrl = baseUrl.Trim();
            if (inputBaseUrl.EndsWith("/"))
                inputBaseUrl = inputBaseUrl.Remove(inputBaseUrl.Length - 1);

            string inputUri = uri?.Trim();
            if (!string.IsNullOrWhiteSpace(inputUri) && inputUri.StartsWith("/"))
                inputUri = inputUri.Remove(0);

            return stringBuilder
                .Append($"{inputBaseUrl}/{inputUri}")
                .Append(' ');
        }

        throw new InvalidDataException("baseUrl argument is null or empty!");
    }

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder, HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders = true)
    {
        bool hasHeader = false;
        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
        {
            foreach (var row in httpClient.DefaultRequestHeaders)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Headers.Any())
        {
            foreach (var row in httpRequestMessage.Headers)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var row in httpRequestMessage.Content.Headers)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{row.Key}: {row.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (!hasHeader)
            stringBuilder.Append(' ');

        return stringBuilder;
    }

    internal static StringBuilder AddBody(this StringBuilder stringBuilder, HttpContent content)
    {
        string contentType = content.Headers.ContentType?.MediaType;
        string body = content.ReadAsStringAsync().GetAwaiter().GetResult();

        if (!_IsValidBody(body, contentType))
            throw new JsonException($"exception in parsing body {contentType}!");

        if (contentType == "application/x-www-form-urlencoded")
            _AddFormUrlEncodedContentBody(stringBuilder, body);
        else
            _AppendBodyItem(stringBuilder, body);

        return stringBuilder;
    }

    private static void _AddFormUrlEncodedContentBody(StringBuilder stringBuilder, string body)
    {
        string decodedBody = HttpUtility.UrlDecode(body);
        string[] splitBodyArray = decodedBody.Split('&');
        if (splitBodyArray.Any())
        {
            foreach (string item in splitBodyArray)
            {
                _AppendBodyItem(stringBuilder, item);
            }
        }
    }

    private static void _AppendBodyItem(StringBuilder stringBuilder, object body)
        => stringBuilder
            .Append("-d")
            .Append(' ')
            .Append('\'')
            .Append(body)
            .Append('\'')
            .Append(' ');

    private static bool _IsValidBody(string body, string contentType)
    {
        switch (contentType)
        {
            case MediaTypeNames.Application.Json when body.IsValidJson() == false:
            case MediaTypeNames.Text.Xml when body.IsValidXml() == false:
                return false;
            default:
                return true;
        }
    }
}