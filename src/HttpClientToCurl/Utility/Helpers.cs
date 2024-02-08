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
            Console.ForegroundColor = SetColor(httpMethod);
            Console.WriteLine(script);
            Console.ResetColor();
        }
    }

    internal static void WriteInFile(string script, string filename, string path)
    {
        path = NormalizedPath(path);
        filename = NormalizedFilename(filename);

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

    private static ConsoleColor SetColor(HttpMethod httpMethod)
    {
        ConsoleColor color;

        if (httpMethod == HttpMethod.Post)
        {
            color = ConsoleColor.Green;
        }
        else if (httpMethod == HttpMethod.Get)
        {
            color = ConsoleColor.Cyan;
        }
        else if (httpMethod == HttpMethod.Put)
        {
            color = ConsoleColor.Yellow;
        }
        else if (httpMethod == HttpMethod.Patch)
        {
            color = ConsoleColor.Magenta;
        }
        else if (httpMethod == HttpMethod.Delete)
        {
            color = ConsoleColor.Red;
        }
        else
        {
            color = ConsoleColor.White;
        }

        return color;
    }

    private static string NormalizedPath(string path)
    {
        string inputPath = path?.Trim();
        if (string.IsNullOrWhiteSpace(inputPath))
        {
            inputPath = Directory.GetCurrentDirectory();
        }

        if (inputPath.EndsWith('/'))
        {
            inputPath = inputPath.Remove(inputPath.Length - 1);
        }

        return inputPath;
    }

    private static string NormalizedFilename(string filename)
    {
        return string.IsNullOrWhiteSpace(filename)
            ? DateTime.Now.Date.ToString("yyyyMMdd")
            : filename.Trim();
    }

    internal static HttpRequestMessage FillHttpRequestMessage(HttpMethod httpMethod, HttpRequestHeaders requestHeaders, HttpContent requestBody, Uri requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage { Method = httpMethod };

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
    {
        return string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
    }
}
