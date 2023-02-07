using System.Net.Http.Headers;
using System.Net.Mime;

namespace HttpClientToCurl.Utility;

public static class Helpers
{
    public static void WriteInConsole(string script, bool enableCodeBeautification, HttpMethod httpMethod)
    {
        if (!enableCodeBeautification)
            Console.WriteLine(script);
        else
        {
            Console.ForegroundColor = httpMethod.SetColor();
            Console.WriteLine(script);
            Console.ResetColor();
        }
    }

    public static void WriteInFile(string script, string filename, string path)
    {
        path = path.NormalizedPath();
        filename = filename.NormalizedFilename();

        string fullPath = $"{path}{Path.DirectorySeparatorChar.ToString()}{filename}.curl";
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

    public static HttpRequestMessage FillHttpRequestMessage(HttpMethod httpMethod, HttpRequestHeaders requestHeaders, HttpContent requestBody)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = httpMethod
        };

        if (requestBody is not null)
            httpRequestMessage.Content = requestBody;

        if (requestHeaders is not null && requestHeaders.Any())
        {
            foreach (var header in requestHeaders)
            {
                httpRequestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        return httpRequestMessage;
    }
    
    public static bool IsValidBody(string body, string contentType)
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