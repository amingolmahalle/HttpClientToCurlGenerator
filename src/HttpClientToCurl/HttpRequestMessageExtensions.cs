using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl;

public static class HttpRequestMessageExtensions
{
    #region :: EXTENSIONS ::

    #region : Put in a variable :

    public static string GenerateCurlInString(this HttpRequestMessage httpRequestMessage, Uri baseAddress, Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
        {
            return string.Empty;
        }

        string script = Generator.GenerateCurl(
            httpRequestMessage,
            baseAddress,
            stringConfig);

        return script;
    }

    #endregion : Put in a variable :

    #region : Show in the console :

    public static void GenerateCurlInConsole(this HttpRequestMessage httpRequestMessage, Uri baseAddress, Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
        {
            return;
        }

        string script = Generator.GenerateCurl(httpRequestMessage, baseAddress, consoleConfig);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    #endregion : Print in the console :

    #region : Write in a file :

    public static void GenerateCurlInFile(this HttpRequestMessage httpRequestMessage, Uri baseAddress, Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
        {
            return;
        }

        string script = Generator.GenerateCurl(httpRequestMessage, baseAddress, fileConfig);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion : Write in a file :

    #endregion :: EXTENSIONS ::
}
