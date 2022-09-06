using Newtonsoft.Json.Linq;

namespace HttpClientToCurl.Utility;

public static class Extensions
{
    public static ConsoleColor SetColor(this HttpMethod httpMethod)
    {
        ConsoleColor color;

        if (httpMethod == HttpMethod.Post)
            color = ConsoleColor.DarkGreen;
        else if (httpMethod == HttpMethod.Get)
            color = ConsoleColor.DarkCyan;
        else if (httpMethod == HttpMethod.Put)
            color = ConsoleColor.DarkYellow;
        else if (httpMethod == HttpMethod.Patch)
            color = ConsoleColor.Yellow;
        else if (httpMethod == HttpMethod.Delete)
            color = ConsoleColor.DarkRed;
        else
            color = ConsoleColor.White;

        return color;
    }


    public static string NormalizePath(this string path)
    {
        string inputPath = path?.Trim();
        if (string.IsNullOrWhiteSpace(inputPath))
            inputPath = inputPath._TryGetSolutionDirectoryInfo();

        if (inputPath.EndsWith('/'))
            inputPath = inputPath.Remove(inputPath.Length - 1);

        return inputPath;
    }

    private static string _TryGetSolutionDirectoryInfo(this string path)
    {
        var directory = new DirectoryInfo(path ?? Directory.GetCurrentDirectory());

        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? Directory.GetCurrentDirectory();
    }

    public static string NormalizeFilename(this string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            filename = DateTime.Now.Date.ToString("yyyyMMdd");

        return filename.Trim();
    }

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
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }
}