using System.Text;
using System.Web;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl.Builder.Concrete.Common;

internal static class Extensions
{
    internal static StringBuilder Initialize(this StringBuilder stringBuilder, HttpMethod httpMethod)
    {
        stringBuilder.Append("curl");

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

    internal static string Finalize(this StringBuilder stringBuilder, bool enableCompression)
    {
        return enableCompression
            ? stringBuilder
                .Append(' ')
                .Append("--compressed")
                .ToString()
            : stringBuilder.ToString();
    }

    internal static StringBuilder AddAbsoluteUrl(this StringBuilder stringBuilder, string baseAddress, Uri requestUri)
    {
        Uri baseAddressUri = Helpers.CreateUri(baseAddress);
        bool baseAddressIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(baseAddressUri);
        bool requestUriIsAbsoluteUri = Helpers.CheckAddressIsAbsoluteUri(requestUri);

        string address = GetAbsoluteAddress(baseAddressUri, baseAddressIsAbsoluteUri, requestUri, requestUriIsAbsoluteUri);

        var encodedAddress = address.ApplyEncodeUri();

        return stringBuilder
            .Append($"{AddSingleQuotationMark(encodedAddress ?? address)}")
            .Append(' ');

        static string AddSingleQuotationMark(string address)
        {
            return address is not null ? $"'{address}'" : null;
        }
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

    private static string ApplyEncodeUri(this string address)
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
                httpClient.DefaultRequestHeaders.Where(dh => dh.Key != Constants.ContentLength);
            foreach (var header in defaultHeaders)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {string.Join("; ", header.Value)}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Headers.Any())
        {
            var headers = httpRequestMessage.Headers.Where(h => h.Key != Constants.ContentLength);
            foreach (var header in headers)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {string.Join("; ", header.Value)}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var header in httpRequestMessage.Content.Headers.Where(h => h.Key != Constants.ContentLength))
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {string.Join("; ", header.Value)}\'")
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

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder,
       HttpRequestMessage httpRequestMessage)
    {
        bool hasHeader = false;

        if (httpRequestMessage.Headers.Any())
        {
            var headers = httpRequestMessage.Headers.Where(h => h.Key != Constants.ContentLength);
            foreach (var header in headers)
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {string.Join("; ", header.Value)}\'")
                    .Append(' ');
            }

            hasHeader = true;
        }

        if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.Any())
        {
            foreach (var header in httpRequestMessage.Content.Headers.Where(h => h.Key != Constants.ContentLength))
            {
                stringBuilder
                    .Append("-H")
                    .Append(' ')
                    .Append($"\'{header.Key}: {string.Join("; ", header.Value)}\'")
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

        if (contentType == Constants.FormUrlEncodedContentType)
        {
            stringBuilder.AddFormUrlEncodedContentBody(body);
        }
        else
        {
            stringBuilder.AppendBodyItem(body);
        }

        return stringBuilder;
    }

    private static void AppendBodyItem(this StringBuilder stringBuilder, object body)
        => stringBuilder
            .Append("-d")
            .Append(' ')
            .Append('\'')
            .Append(body)
            .Append('\'')
            .Append(' ');

    private static void AddFormUrlEncodedContentBody(this StringBuilder stringBuilder, string body)
    {
        string decodedBody = HttpUtility.UrlDecode(body);
        string[] splitBodyArray = decodedBody.Split('&');
        if (splitBodyArray.Any())
        {
            foreach (string item in splitBodyArray)
            {
                stringBuilder.AppendBodyItem(item);
            }
        }
    }
}
