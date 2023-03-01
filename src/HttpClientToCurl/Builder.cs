using System.Net;
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

        return stringBuilder.Append(' ');
    }

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string baseUrl, string requestUri)
    {
        bool hasSlashEndOfBaseUrl = false;
        bool hasSlashFirstOfRequestUri = false;
        string splitterUrl = string.Empty;

        string inputBaseUrl = baseUrl?.Trim();
        if (!string.IsNullOrWhiteSpace(inputBaseUrl))
        {
            if (inputBaseUrl.EndsWith('/'))
                hasSlashEndOfBaseUrl = true;

            string inputRequestUri = requestUri?.Trim();
            if (!string.IsNullOrWhiteSpace(inputRequestUri))
            {
                if (inputRequestUri.StartsWith('/'))
                    hasSlashFirstOfRequestUri = true;

                string warningMessage = CheckAndAddWarningMessageForIncorrectSlash(hasSlashEndOfBaseUrl, hasSlashFirstOfRequestUri, out splitterUrl);
                if (!string.IsNullOrEmpty(warningMessage))
                {
                    stringBuilder
                        .Insert(0, warningMessage)
                        .Insert(warningMessage.Length, Environment.NewLine);
                }
            }

            if (hasSlashEndOfBaseUrl && (string.IsNullOrEmpty(inputRequestUri) || hasSlashFirstOfRequestUri))
                inputBaseUrl = inputBaseUrl.Remove(inputBaseUrl.Length - 1);

            return stringBuilder
                .Append($"{inputBaseUrl}{splitterUrl}{inputRequestUri}")
                .Append(' ');
        }

        throw new InvalidDataException("baseUrl argument is null or empty!");

        string CheckAndAddWarningMessageForIncorrectSlash(bool hasOnBaseUrl, bool hasOnRequestUri, out string splitter)
        {
            string message = string.Empty;
            splitter = string.Empty;

            if (hasOnBaseUrl && hasOnRequestUri)
                message = "# Warning: you must remove the Slash at the end of base url or at the first of the requestUri.";
            else if (!hasOnBaseUrl && !hasOnRequestUri)
            {
                splitter = "/";
                message = "# Warning: you must add the Slash at the end of base url or at the first of the requestUri.";
            }

            return message;
        }
    }

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder, HttpClient httpClient, HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders = true)
    {
        bool hasHeader = false;
        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
        {
            var defaultHeaders = httpClient.DefaultRequestHeaders.Where(dh => dh.Key != HttpRequestHeader.ContentLength.ToString());
            foreach (var header in defaultHeaders)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {header.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Headers.Any())
        {
            var headers = httpRequestMessage.Headers.Where(h => h.Key != HttpRequestHeader.ContentLength.ToString());
            foreach (var header in headers)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {header.Value.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var header in httpRequestMessage.Content.Headers.Where(h => h.Key != HttpRequestHeader.ContentLength.ToString()))
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {header.Value.FirstOrDefault()}\'")
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
        string contentType = content?.Headers?.ContentType?.MediaType;
        string body = content?.ReadAsStringAsync().GetAwaiter().GetResult();

        if (content is not null && !string.IsNullOrWhiteSpace(body) && !Helpers.IsValidBody(body, contentType))
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
}