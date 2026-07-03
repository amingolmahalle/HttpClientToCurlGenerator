using HttpClientToCurl.Config;
using HttpClientToCurl.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HttpClientToCurl.HttpMessageHandlers;

public class CurlGeneratorHttpMessageHandler(
    IOptionsMonitor<CompositConfig> monitorConfig,
    ILogger<CurlGeneratorHttpMessageHandler> logger = null) : DelegatingHandler
{
    private readonly IOptionsMonitor<CompositConfig> _monitorConfig = monitorConfig;
    private readonly ILogger<CurlGeneratorHttpMessageHandler> _logger = logger;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        var config = _monitorConfig.CurrentValue;
        if (config.Enable)
        {
            if (config.ShowOnConsole?.TurnOn ?? false)
            {
                httpRequestMessage.GenerateCurlInConsole(httpRequestMessage.RequestUri, consoleConfig =>
                {
                    consoleConfig.TurnOn = true;
                    consoleConfig.EnableCodeBeautification = config.ShowOnConsole.EnableCodeBeautification;
                    consoleConfig.EnableCompression = config.ShowOnConsole.EnableCompression;
                    consoleConfig.NeedAddDefaultHeaders = config.ShowOnConsole.NeedAddDefaultHeaders;
                });
            }

            if (config.SaveToFile?.TurnOn ?? false)
            {
                httpRequestMessage.GenerateCurlInFile(httpRequestMessage.RequestUri, fileConfig =>
                {
                    fileConfig.TurnOn = true;
                    fileConfig.EnableCompression = config.SaveToFile.EnableCompression;
                    fileConfig.NeedAddDefaultHeaders = config.SaveToFile.NeedAddDefaultHeaders;
                    fileConfig.Path = config.SaveToFile.Path;
                    fileConfig.Filename = config.SaveToFile.Filename;
                });
            }

            if (config.SendToLogger?.TurnOn ?? false)
            {
                if (_logger != null)
                {
                    var curl = httpRequestMessage.GenerateCurlInString(httpRequestMessage.RequestUri, stringConfig =>
                    {
                        stringConfig.TurnOn = true;
                        stringConfig.EnableCompression = config.SendToLogger.EnableCompression;
                        stringConfig.NeedAddDefaultHeaders = config.SendToLogger.NeedAddDefaultHeaders;
                    });

                    _logger.Log(config.SendToLogger.LogLevel, curl);
                }
            }
        }

        var response = base.SendAsync(httpRequestMessage, cancellationToken);
        return response;
    }
}
