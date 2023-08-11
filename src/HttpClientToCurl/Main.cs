using System.Net.Http.Headers;
using HttpClientToCurl.Config;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl;

public static class Main
{
    #region :: EXTENSIONS ::

    #region : Put in a variable :

    public static string GenerateCurlInString(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
            return string.Empty;

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            stringConfig.NeedAddDefaultHeaders);

        return script;
    }

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = "",
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
            return string.Empty;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, stringConfig.NeedAddDefaultHeaders);

        return script;
    }

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
            return string.Empty;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, stringConfig.NeedAddDefaultHeaders);

        return script;
    }

    #endregion : Put in a variable :

    #region : Print in the console :

    public static void GenerateCurlInConsole(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = "",
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    #endregion : Print in the console :

    #region : Print in a file :

    public static void GenerateCurlInFile(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri="",
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody, requestUri);

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion : Print in a file :

    #endregion :: EXTENSIONS ::
}