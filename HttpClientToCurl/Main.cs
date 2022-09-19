using HttpClientToCurl.Config;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl;

public static class Main
{
    #region :: EXTENSIONS ::

    public static void GenerateCurlInConsole(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, string requestUri = null, Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri ?? httpRequestMessage.RequestUri?.ToString(), consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInFile(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, string requestUri, Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri, fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion
}