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

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        // Curl generation is a diagnostic side effect: it must never fail the real request.
        await BufferBodyIfNeededAsync(httpRequestMessage).ConfigureAwait(false);
        TryGenerateCurl(httpRequestMessage);

        return await base.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// The curl builders read the body synchronously. Buffering it asynchronously first means that
    /// read completes immediately, so a single-threaded SynchronizationContext cannot deadlock.
    /// Buffered content is reused (not consumed) when the request is actually sent.
    /// </summary>
    private async Task BufferBodyIfNeededAsync(HttpRequestMessage httpRequestMessage)
    {
        if (httpRequestMessage.Content is null || !IsAnySinkOn(_monitorConfig.CurrentValue))
        {
            return;
        }

        try
        {
            switch (httpRequestMessage.Content)
            {
                // Binary bodies and multipart file parts are rendered as placeholders, so there is no need to read them.
                case var content when Builder.Concrete.Common.Extensions.IsBinaryMediaType(content.Headers.ContentType?.MediaType):
                    break;
                case MultipartFormDataContent multipart:
                    foreach (var part in multipart.Where(p => p.Headers.ContentDisposition?.FileName is null
                                                              && p.Headers.ContentDisposition?.FileNameStar is null))
                    {
                        await part.LoadIntoBufferAsync().ConfigureAwait(false);
                    }

                    break;
                case var content:
                    await content.LoadIntoBufferAsync().ConfigureAwait(false);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "HttpClientToCurl failed to read the request body; the request was still sent.");
        }
    }

    private static bool IsAnySinkOn(CompositConfig config)
        => config.Enable
           && ((config.ShowOnConsole?.TurnOn ?? false)
               || (config.SaveToFile?.TurnOn ?? false)
               || (config.SendToLogger?.TurnOn ?? false));

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
