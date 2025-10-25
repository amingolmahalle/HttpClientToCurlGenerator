using HttpClientToCurl.Config;
using HttpClientToCurl.Config.Others;
using HttpClientToCurl.Extensions;

namespace HttpClientToCurl.HttpMessageHandlers;

public class CurlGeneratorHttpMessageHandler(GlobalConfig config) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        if (config.ShowMode.HasFlag(ShowMode.Console))
        {
            httpRequestMessage.GenerateCurlInConsole(httpRequestMessage.RequestUri, consoleConfig =>
            {
                consoleConfig.TurnOn = true;
                consoleConfig.EnableCodeBeautification = config.ConsoleEnableCodeBeautification;
                consoleConfig.EnableCompression = config.ConsoleEnableCompression;
                consoleConfig.NeedAddDefaultHeaders = config.NeedAddDefaultHeaders;
            });
        }

        if (config.ShowMode.HasFlag(ShowMode.File))
        {
            httpRequestMessage.GenerateCurlInFile(httpRequestMessage.RequestUri, fileConfig =>
            {
                fileConfig.TurnOn = true;
                fileConfig.Filename = config.FileConfigFileName;
                fileConfig.Path = config.FileConfigPath;
                fileConfig.NeedAddDefaultHeaders = config.NeedAddDefaultHeaders;
            });
        }

        var response = base.SendAsync(httpRequestMessage, cancellationToken);
        return response;
    }
}
