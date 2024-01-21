using System.Net.Http.Headers;

namespace HttpClientToCurl.Utility;

public static class Helpers
{
    internal static void WriteInConsole(string script, bool enableCodeBeautification, HttpMethod httpMethod)
    {
        if (!enableCodeBeautification)
        {
            Console.WriteLine(script);
        }
        else
        {
            Console.ForegroundColor = httpMethod.SetColor();
            Console.WriteLine(script);
            Console.ResetColor();
        }
    }

    internal static void WriteInFile(string script, string filename, string path)
    {
        path = path.NormalizedPath();
        filename = filename.NormalizedFilename();

        string fullPath = $"{path}{Path.DirectorySeparatorChar}{filename}.curl";
        if (File.Exists(fullPath))
        {
            using var streamWriter = new StreamWriter(fullPath, true);
            streamWriter.WriteLine(script);
        }
        else
        {
            using var streamWriter = File.CreateText(fullPath);
            streamWriter.WriteLine(script);
        }
    }

    internal static HttpRequestMessage FillHttpRequestMessage(HttpMethod httpMethod, HttpRequestHeaders requestHeaders, HttpContent requestBody, Uri requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = httpMethod
        };

        if (requestBody is not null)
        {
            httpRequestMessage.Content = requestBody;
        }

        if (requestHeaders is not null && requestHeaders.Any())
        {
            foreach (var header in requestHeaders)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        httpRequestMessage.RequestUri = requestUri;

        return httpRequestMessage;
    }

    internal static HttpRequestMessage FillHttpRequestMessage(HttpMethod httpMethod, HttpRequestHeaders requestHeaders, HttpContent requestBody, string requestUri)
    {
        var httpRequestMessage = FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, CreateUri(requestUri));

        return httpRequestMessage;
    }

    internal static bool CheckAddressIsAbsoluteUri(Uri baseAddress)
    {
        bool isValidAbsoluteAddress = true;

        if (baseAddress is null)
        {
            isValidAbsoluteAddress = false;
        }
        else if (!baseAddress.IsAbsoluteUri)
        {
            isValidAbsoluteAddress = false;
        }
        else if (!IsHttpUri(baseAddress))
        {
            isValidAbsoluteAddress = false;
        }

        return isValidAbsoluteAddress;
    }

    private static bool IsHttpUri(Uri uri)
    {
        string scheme = uri.Scheme;

        return (string.Compare("http", scheme, StringComparison.OrdinalIgnoreCase) == 0) ||
               (string.Compare("https", scheme, StringComparison.OrdinalIgnoreCase) == 0);
    }

    public static Uri CreateUri(string uri)
        => string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
}
