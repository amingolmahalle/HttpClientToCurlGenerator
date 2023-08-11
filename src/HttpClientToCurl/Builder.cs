using System.Net;
using System.Text;
using HttpClientToCurl.Utility;

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

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string inputBaseAddress, Uri inputRequestUri)
    {
        Uri requestUri = null;
        Uri baseAddressUri = Helpers.CreateUri(inputBaseAddress);
        bool baseAddressIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(baseAddressUri);
        bool requestUriIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(inputRequestUri);

        if (inputRequestUri is null && baseAddressUri is not null && baseAddressIsAbsoluteUri)
            requestUri = baseAddressUri;
        else if (baseAddressUri is null && inputRequestUri is not null && requestUriIsAbsoluteUri)
            requestUri = inputRequestUri;
        else if (baseAddressUri is not null && inputRequestUri is not null && baseAddressIsAbsoluteUri && !requestUriIsAbsoluteUri)
            requestUri = new Uri(baseAddressUri, inputRequestUri);
        else if (baseAddressUri is not null && inputRequestUri is not null && baseAddressIsAbsoluteUri)
            requestUri = inputRequestUri;

        return stringBuilder
            .Append($"{requestUri}")
            .Append(' ');
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
                    .Append($"\'{header.Key}: {header.Value?.FirstOrDefault()}\'")
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
                    .Append($"\'{header.Key}: {header.Value?.FirstOrDefault()}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var header in httpRequestMessage.Content.Headers.Where(h => h.Key != HttpRequestHeader.ContentLength.ToString()))
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {header.Value?.FirstOrDefault()}\'")
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

        if (contentType == "application/x-www-form-urlencoded")
            stringBuilder.AddFormUrlEncodedContentBody(body);
        else
            stringBuilder.AppendBodyItem(body);

        return stringBuilder;
    }
}