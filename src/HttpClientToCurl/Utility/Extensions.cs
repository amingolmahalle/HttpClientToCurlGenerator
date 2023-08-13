using System.Text;
using System.Web;

namespace HttpClientToCurl.Utility;

internal static class Extensions
{
    internal static void AppendBodyItem(this StringBuilder stringBuilder, object body)
        => stringBuilder
            .Append("-d")
            .Append(' ')
            .Append('\'')
            .Append(body)
            .Append('\'')
            .Append(' ');

    internal static void AddFormUrlEncodedContentBody(this StringBuilder stringBuilder, string body)
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

    internal static ConsoleColor SetColor(this HttpMethod httpMethod)
    {
        ConsoleColor color;

        if (httpMethod == HttpMethod.Post)
            color = ConsoleColor.Green;
        else if (httpMethod == HttpMethod.Get)
            color = ConsoleColor.Cyan;
        else if (httpMethod == HttpMethod.Put)
            color = ConsoleColor.Yellow;
        else if (httpMethod == HttpMethod.Patch)
            color = ConsoleColor.Magenta;
        else if (httpMethod == HttpMethod.Delete)
            color = ConsoleColor.Red;
        else
            color = ConsoleColor.White;

        return color;
    }

    internal static string NormalizedPath(this string path)
    {
        string inputPath = path?.Trim();
        if (string.IsNullOrWhiteSpace(inputPath))
            inputPath = Directory.GetCurrentDirectory();

        if (inputPath.EndsWith('/'))
            inputPath = inputPath.Remove(inputPath.Length - 1);

        return inputPath;
    }

    internal static string NormalizedFilename(this string filename)
        => string.IsNullOrWhiteSpace(filename)
            ? DateTime.Now.Date.ToString("yyyyMMdd")
            : filename.Trim();
}