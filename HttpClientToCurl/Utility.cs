namespace HttpClientToCurl;

internal static class Utility
{
    internal static void WriteInConsole(string script, bool enableCodeBeautification, HttpMethod httpMethod)
    {
        if (!enableCodeBeautification)
            Console.WriteLine(script);
        else
        {
            Console.ForegroundColor = _SetColor(httpMethod);
            Console.WriteLine(script);
            Console.ResetColor();
        }
    }

    private static ConsoleColor _SetColor(HttpMethod httpMethod)
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

    internal static void WriteInFile(string script, string filename, string path)
    {
        path = _NormalizePath(path);

        filename = _NormalizeFilename(filename);

        string fullPath = $"{path}/{filename}.curl";


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

    private static string _NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path?.Trim()))
            path = _TryGetSolutionDirectoryInfo(path) ?? Directory.GetCurrentDirectory();

        if (path.EndsWith('/'))
            path.Remove(path.Length - 1);

        return path;
    }

    private static string _TryGetSolutionDirectoryInfo(string path)
    {
        var directory = new DirectoryInfo(path ?? Directory.GetCurrentDirectory());

        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName;
    }

    private static string _NormalizeFilename(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename?.Trim()))
            filename = DateTime.Now.Date.ToString("yyyyMMdd");

        return filename;
    }
}