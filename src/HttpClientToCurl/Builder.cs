using System.Net;
using System.Text;
using System.Web;
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
        bool baseAddressIsAbsoluteUri = CheckAddressIsAbsoluteUri(baseAddressUri);
        bool requestUriIsAbsoluteUri = CheckAddressIsAbsoluteUri(inputRequestUri);
        
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

    private static bool CheckAddressIsAbsoluteUri(Uri baseAddress)
    {
        bool isValidAbsoluteAddress = true;
        
        if (baseAddress is null)
            isValidAbsoluteAddress = false;
        else if (!baseAddress.IsAbsoluteUri)
            isValidAbsoluteAddress = false;
        else if (!Helpers.IsHttpUri(baseAddress))
            isValidAbsoluteAddress = false;

        return isValidAbsoluteAddress;
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

        if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers.Any())
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