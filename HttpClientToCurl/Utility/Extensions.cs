using System.Xml;
using Newtonsoft.Json.Linq;

namespace HttpClientToCurl.Utility;

public static class Extensions
{
    public static ConsoleColor SetColor(this HttpMethod httpMethod)
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


    public static string NormalizedPath(this string path)
    {
        string inputPath = path?.Trim();
        if (string.IsNullOrWhiteSpace(inputPath))
            inputPath = Directory.GetCurrentDirectory();

        if (inputPath.EndsWith('/'))
            inputPath = inputPath.Remove(inputPath.Length - 1);

        return inputPath;
    }

    public static string NormalizedFilename(this string filename)
        => string.IsNullOrWhiteSpace(filename)
            ? DateTime.Now.Date.ToString("yyyyMMdd")
            : filename.Trim();
    
    public static bool IsValidJson(this string stringInput)
    {
        if (string.IsNullOrWhiteSpace(stringInput))
            return false;

        stringInput = stringInput.Trim();
        if ((stringInput.StartsWith("{") && stringInput.EndsWith("}")) || // For object
            (stringInput.StartsWith("[") && stringInput.EndsWith("]"))) //  For array
        {
            try
            {
                JToken.Parse(stringInput);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public static bool IsValidXml(this string stringInput)
    {
        try
        {
            new XmlDocument().LoadXml(stringInput);
            return true;
        }
        catch
        {
            return false;
        }
    }
}