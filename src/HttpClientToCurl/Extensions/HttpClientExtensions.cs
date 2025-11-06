using System.Net.Http.Headers;
using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl.Extensions;

public static class HttpClientExtensions
{
    #region :: EXTENSIONS ::

    #region : Put in a variable :

    public static string GenerateCurlInString(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, stringConfig, config);

        return script;
    }

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = "",
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<StringConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var stringConfig = new StringConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, stringConfig, config);

        return script;
    }

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<StringConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var stringConfig = new StringConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, stringConfig, config);

        return script;
    }

    #endregion : Put in a variable :

    #region : Show in the console :

    public static void GenerateCurlInConsole(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, consoleConfig, config);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = "",
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<ConsoleConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var consoleConfig = new ConsoleConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, consoleConfig, config);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<ConsoleConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var consoleConfig = new ConsoleConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, consoleConfig, config);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    #endregion : Print in the console :

    #region : Write in a file :

    public static void GenerateCurlInFile(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, fileConfig, config);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = "",
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<FileConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var fileConfig = new FileConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, fileConfig, config);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        Uri requestUri,
        HttpRequestHeaders httpRequestHeaders = null,
        HttpContent httpContent = null,
        Action<FileConfig> config = null)
    {
        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, httpRequestHeaders, httpContent, requestUri);
        var fileConfig = new FileConfig();
        var script = GenerateCurlBaseOnConfig(httpClient, httpRequestMessage, fileConfig, config);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion : Write in a file :\

    #region : Private methods :
    private static string GenerateCurlBaseOnConfig<TConfig>(HttpClient httpClient, HttpRequestMessage httpRequestMessage, TConfig config, Action<TConfig> configAction) where TConfig : BaseConfig
    {
        configAction?.Invoke(config);
        return config.TurnOn ? Generator.GenerateCurl(httpClient, httpRequestMessage, config) : string.Empty;
    }

    #endregion : Private methods :

    #endregion :: EXTENSIONS ::
}
