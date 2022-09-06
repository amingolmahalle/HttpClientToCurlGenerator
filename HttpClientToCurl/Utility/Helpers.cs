namespace HttpClientToCurl.Utility;

public static class Helpers
{
    internal static void WriteInConsole(string script, bool enableCodeBeautification, HttpMethod httpMethod)
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

    internal static void WriteInFile(string script, string filename, string path)
    {
        path = path.NormalizePath();
        filename = filename.NormalizeFilename();

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
}