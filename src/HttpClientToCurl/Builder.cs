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

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string baseAddress, Uri requestUri)
    {
        Uri baseAddressUri = Helpers.CreateUri(baseAddress);
        bool baseAddressIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(baseAddressUri);
        bool requestUriIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(requestUri);

        string address = GetAbsoluteAddress(baseAddressUri, baseAddressIsAbsoluteUri, requestUri, requestUriIsAbsoluteUri);

        var encodedAddress = ApplyEncodeUri(address);

        return stringBuilder
            .Append($"{encodedAddress ?? address}")
            .Append(' ');
    }

    private static string GetAbsoluteAddress(Uri baseAddressUri, bool baseAddressIsAbsoluteUri, Uri requestUri, bool requestUriIsAbsoluteUri)
    {
        if (requestUri is null && baseAddressUri is not null && baseAddressIsAbsoluteUri)
        {
            return baseAddressUri.ToString();
        }

        if (baseAddressUri is null && requestUri is not null && requestUriIsAbsoluteUri)
        {
            return requestUri.ToString();
        }

        if (baseAddressUri is not null && requestUri is not null && baseAddressIsAbsoluteUri && !requestUriIsAbsoluteUri)
        {
            return new Uri(baseAddressUri, requestUri).ToString();
        }

        if (baseAddressUri is not null && requestUri is not null && baseAddressIsAbsoluteUri)
        {
            return requestUri.ToString();
        }

        if (baseAddressUri is null && requestUri is null)
        {
            return null;
        }

        return $"{baseAddressUri}{requestUri}";
    }

    private static string ApplyEncodeUri(string address)
    {
        string result = null;

        if (address is not null)
        {
            var questionMarkItems = address.Split('?');
            if (questionMarkItems.Length > 1)
            {
                var andItems = questionMarkItems[1].Split('&');
                if (andItems.Length > 1)
                {
                    var addressEncodedStringBuilder = new StringBuilder()
                        .Append(questionMarkItems[0])
                        .Append('?');
                    foreach (var ai in andItems)
                    {
                        var equalItems = ai.Split('=');
                        if (equalItems.Length > 1)
                        {
                            addressEncodedStringBuilder
                                .Append(equalItems[0])
                                .Append('=')
                                .Append(Uri.EscapeDataString(equalItems[1]))
                                .Append('&');
                        }
                    }

                    result = addressEncodedStringBuilder
                        .Remove(addressEncodedStringBuilder.Length - 1, 1)
                        .ToString();
                }
            }
        }

        return result;
    }

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder, HttpClient httpClient,
        HttpRequestMessage httpRequestMessage, bool needAddDefaultHeaders = true)
    {
        bool hasHeader = false;

        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
        {
            var defaultHeaders =
                httpClient.DefaultRequestHeaders.Where(dh => dh.Key != HttpRequestHeader.ContentLength.ToString());
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
            foreach (var header in httpRequestMessage.Content.Headers.Where(h =>
                         h.Key != HttpRequestHeader.ContentLength.ToString()))
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
        {
            stringBuilder.Append(' ');
        }

        return stringBuilder;
    }

    internal static StringBuilder AddBody(this StringBuilder stringBuilder, HttpContent content)
    {
        string contentType = content?.Headers?.ContentType?.MediaType;
        string body = content?.ReadAsStringAsync().GetAwaiter().GetResult();

        if (contentType == "application/x-www-form-urlencoded")
        {
            stringBuilder.AddFormUrlEncodedContentBody(body);
        }
        else
        {
            stringBuilder.AppendBodyItem(body);
        }

        return stringBuilder;
    }
}
