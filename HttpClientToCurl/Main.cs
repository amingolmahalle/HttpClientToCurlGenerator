using HttpClientToCurl.Config;

namespace HttpClientToCurl;

public static class Main
{
    #region :: EXTENSIONS ::

    public static void GenerateCurlInConsole(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, string requestUri, Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri, consoleConfig.NeedAddDefaultHeaders);

        Utility.WriteInConsole(script);
    }

    public static void GenerateCurlInFile(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, string requestUri, Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri, fileConfig.NeedAddDefaultHeaders);

        Utility._WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion
}