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
            return address is not null ? $"'{EscapeSingleQuotes(address)}'" : null;
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
        if (address is null)
        {
            return null;
        }

        int queryStart = address.IndexOf('?');
        if (queryStart < 0)
        {
            return null;
        }

        int fragmentStart = address.IndexOf('#', queryStart);
        string path = address[..queryStart];
        string query = fragmentStart < 0 ? address[(queryStart + 1)..] : address[(queryStart + 1)..fragmentStart];
        string fragment = fragmentStart < 0 ? string.Empty : address[fragmentStart..];

        var parameters = query
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(EncodeQueryParameter)
            .ToArray();

        return parameters.Length == 0
            ? null
            : $"{path}?{string.Join("&", parameters)}{fragment}";
    }

    /// <summary>
    /// Encodes the value of one "name=value" pair exactly once: the value is split off at the first '=',
    /// decoded (so existing %XX sequences are not double-encoded) and then escaped.
    /// Pairs without '=' are returned unchanged.
    /// </summary>
    private static string EncodeQueryParameter(string parameter)
    {
        int equalsIndex = parameter.IndexOf('=');
        if (equalsIndex < 0)
        {
            return parameter;
        }

        string name = parameter[..equalsIndex];
        string value = parameter[(equalsIndex + 1)..];

        // A literal '+' means "space" on the wire; only an encoded %2B is a real plus. Keep literal pluses as they are.
        var encodedPieces = value.Split('+').Select(piece => Uri.EscapeDataString(Uri.UnescapeDataString(piece)));

        return $"{name}={string.Join("+", encodedPieces)}";
    }

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder, HttpClient httpClient,
        HttpRequestMessage httpRequestMessage, string[] redactedHeaders, bool needAddDefaultHeaders = true)
    {
        bool hasHeader = false;

        if (needAddDefaultHeaders && httpClient.DefaultRequestHeaders.Any())
        {
            AppendHeaderLines(stringBuilder, httpClient.DefaultRequestHeaders, redactedHeaders);
            hasHeader = true;
        }

        hasHeader |= AppendRequestAndContentHeaders(stringBuilder, httpRequestMessage, redactedHeaders);

        if (!hasHeader)
        {
            stringBuilder.Append(' ');
        }

        return stringBuilder;
    }

    internal static StringBuilder AddHeaders(this StringBuilder stringBuilder,
        HttpRequestMessage httpRequestMessage,
        string[] redactedHeaders)
    {
        bool hasHeader = AppendRequestAndContentHeaders(stringBuilder, httpRequestMessage, redactedHeaders);

        if (!hasHeader)
        {
            stringBuilder.Append(' ');
        }

        return stringBuilder;
    }

    private static bool AppendRequestAndContentHeaders(
        StringBuilder stringBuilder, HttpRequestMessage httpRequestMessage, string[] redactedHeaders)
    {
        bool hasHeader = false;

        if (httpRequestMessage.Headers.Any())
        {
            AppendHeaderLines(stringBuilder, httpRequestMessage.Headers, redactedHeaders);
            hasHeader = true;
        }

        if (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.Any())
        {
            // For multipart/form-data curl builds its own Content-Type (and boundary) from the -F parts.
            bool skipContentType = httpRequestMessage.Content is MultipartFormDataContent;
            AppendHeaderLines(stringBuilder, httpRequestMessage.Content.Headers, redactedHeaders, skipContentType);
            hasHeader = true;
        }

        return hasHeader;
    }

    private static void AppendHeaderLines(
        StringBuilder stringBuilder,
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
        string[] redactedHeaders,
        bool skipContentType = false)
    {
        foreach (var header in headers)
        {
            if (header.Key == Constants.ContentLength || (skipContentType && header.Key == Constants.ContentType))
            {
                continue;
            }

            string headerValue = redactedHeaders is not null && redactedHeaders.Contains(header.Key, StringComparer.OrdinalIgnoreCase)
                ? "[REDACTED]"
                : string.Join(GetValueSeparator(header.Key), header.Value);

            stringBuilder
                .Append("-H")
                .Append(' ')
                .Append($"'{EscapeSingleQuotes($"{header.Key}: {headerValue}")}'")
                .Append(' ');
        }
    }

    /// <summary>
    /// Multiple values of one header are comma separated on the wire, except User-Agent
    /// (product tokens and comments are space separated) and Cookie (pairs are "; " separated).
    /// </summary>
    private static string GetValueSeparator(string headerName)
        => headerName.ToUpperInvariant() switch
        {
            "USER-AGENT" => " ",
            "COOKIE" => "; ",
            _ => ", ",
        };

    /// <summary>
    /// Makes a value safe inside a single-quoted shell string: ' becomes '\''.
    /// </summary>
    internal static string EscapeSingleQuotes(string value)
        => value?.Replace("'", "'\\''");

    internal static StringBuilder AddBody(this StringBuilder stringBuilder, HttpContent content)
    {
        if (content is MultipartFormDataContent multipartContent)
        {
            return stringBuilder.AddMultipartFormBody(multipartContent);
        }

        string contentType = content?.Headers?.ContentType?.MediaType;

        if (content is not null && IsBinaryMediaType(contentType))
        {
            return stringBuilder
                .Append("--data-binary")
                .Append(' ')
                .Append($"'@{Constants.BinaryPayloadPlaceholder}'")
                .Append(' ');
        }

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

    private static readonly string[] BinaryMediaTypePrefixes = ["image/", "audio/", "video/", "font/"];

    private static readonly string[] BinaryMediaTypes =
    [
        "application/octet-stream", "application/pdf", "application/zip", "application/gzip", "application/x-gzip",
        "application/x-tar", "application/x-protobuf", "application/protobuf", "application/msgpack",
        "application/x-msgpack", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    ];

    internal static bool IsBinaryMediaType(string mediaType)
        => mediaType is not null
           && (BinaryMediaTypePrefixes.Any(prefix => mediaType.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
               || BinaryMediaTypes.Contains(mediaType, StringComparer.OrdinalIgnoreCase));

    /// <summary>
    /// Text parts become --form-string (so a leading '@' or '&lt;' is never treated as a file reference);
    /// file parts become -F name=@filename, which the reader must point at a real local file.
    /// </summary>
    private static StringBuilder AddMultipartFormBody(this StringBuilder stringBuilder, MultipartFormDataContent content)
    {
        foreach (var part in content)
        {
            var disposition = part.Headers.ContentDisposition;
            string name = disposition?.Name?.Trim('"') ?? string.Empty;
            string fileName = (disposition?.FileNameStar ?? disposition?.FileName)?.Trim('"');

            if (fileName is not null)
            {
                string mediaType = part.Headers.ContentType?.MediaType;
                string field = $"{name}=@{QuoteFileName(fileName)}" + (mediaType is null ? string.Empty : $";type={mediaType}");
                stringBuilder.Append("-F").Append(' ').Append('\'').Append(EscapeSingleQuotes(field)).Append('\'').Append(' ');
            }
            else
            {
                string value = part.ReadAsStringAsync().GetAwaiter().GetResult();
                stringBuilder
                    .Append("--form-string")
                    .Append(' ')
                    .Append('\'')
                    .Append(EscapeSingleQuotes($"{name}={value}"))
                    .Append('\'')
                    .Append(' ');
            }
        }

        return stringBuilder;
    }

    private static string QuoteFileName(string fileName)
        => fileName.IndexOfAny([';', ',', '"']) < 0
            ? fileName
            : $"\"{fileName.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    private static void AppendBodyItem(this StringBuilder stringBuilder, object body)
    {
        string text = body?.ToString();

        // curl -d treats a leading '@' as "read the data from this file"; --data-raw sends it literally.
        stringBuilder
            .Append(text is not null && text.StartsWith('@') ? "--data-raw" : "-d")
            .Append(' ')
            .Append('\'')
            .Append(EscapeSingleQuotes(text))
            .Append('\'')
            .Append(' ');
    }

    /// <summary>
    /// The body is split into fields first and only then decoded, so an encoded '&amp;' inside a value
    /// cannot start a new field. Values that would change meaning once decoded ('&amp;', '+', '%') are
    /// emitted with --data-urlencode so curl encodes them again.
    /// </summary>
    private static void AddFormUrlEncodedContentBody(this StringBuilder stringBuilder, string body)
    {
        foreach (string field in body.Split('&'))
        {
            int equalsIndex = field.IndexOf('=');
            if (equalsIndex < 0)
            {
                stringBuilder.AppendBodyItem(HttpUtility.UrlDecode(field));
                continue;
            }

            string name = HttpUtility.UrlDecode(field[..equalsIndex]);
            string value = HttpUtility.UrlDecode(field[(equalsIndex + 1)..]);

            if (value.IndexOfAny(['&', '+', '%']) >= 0)
            {
                stringBuilder
                    .Append("--data-urlencode")
                    .Append(' ')
                    .Append('\'')
                    .Append(EscapeSingleQuotes($"{name}={value}"))
                    .Append('\'')
                    .Append(' ');
            }
            else
            {
                stringBuilder.AppendBodyItem($"{name}={value}");
            }
        }
    }
}
