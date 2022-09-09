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
}