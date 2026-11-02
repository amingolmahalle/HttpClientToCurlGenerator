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
        // Curl generation is a diagnostic side effect: it must never fail the real request.
        TryGenerateCurl(httpRequestMessage);

        return base.SendAsync(httpRequestMessage, cancellationToken);
    }

    private void TryGenerateCurl(HttpRequestMessage httpRequestMessage)
    {
        var config = _monitorConfig.CurrentValue;
        if (!config.Enable)
        {
            return;
        }

        if (config.ShowOnConsole?.TurnOn ?? false)
        {
            Safely(() => httpRequestMessage.GenerateCurlInConsole(httpRequestMessage.RequestUri, consoleConfig =>
            {
                consoleConfig.CopyCommonOptionsFrom(config.ShowOnConsole);
                consoleConfig.EnableCodeBeautification = config.ShowOnConsole.EnableCodeBeautification;
            }));
        }

        if (config.SaveToFile?.TurnOn ?? false)
        {
            Safely(() => httpRequestMessage.GenerateCurlInFile(httpRequestMessage.RequestUri, fileConfig =>
            {
                fileConfig.CopyCommonOptionsFrom(config.SaveToFile);
                fileConfig.Path = config.SaveToFile.Path;
                fileConfig.Filename = config.SaveToFile.Filename;
            }));
        }

        if ((config.SendToLogger?.TurnOn ?? false) && _logger != null)
        {
            Safely(() =>
            {
                var curl = httpRequestMessage.GenerateCurlInString(httpRequestMessage.RequestUri, stringConfig =>
                {
                    stringConfig.CopyCommonOptionsFrom(config.SendToLogger);
                });

                _logger.Log(config.SendToLogger.LogLevel, curl);
            });
        }
    }

    private void Safely(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "HttpClientToCurl failed to generate the curl command; the request was still sent.");
        }
    }
}
