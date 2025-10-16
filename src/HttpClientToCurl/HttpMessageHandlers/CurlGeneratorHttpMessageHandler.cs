using HttpClientToCurl.Config;
using HttpClientToCurl.Extensions;

namespace HttpClientToCurl.HttpMessageHandlers;

public class CurlGeneratorHttpMessageHandler(GlobalConfig config) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (config.TurnOnAll)
        {
            var consoleConfig = new ConsoleConfig();
            config.ShowOnConsole?.Invoke(consoleConfig);
            if (consoleConfig.TurnOn)
            {
                HttpRequestMessageExtensions.GenerateCurlInConsole(request, null);
            }

            var fileConfig = new FileConfig();
            config.SaveToFile?.Invoke(fileConfig);
            if (fileConfig.TurnOn)
            {
                HttpRequestMessageExtensions.GenerateCurlInFile(request, null, config.SaveToFile);
            }
        }

        var response = base.SendAsync(request, cancellationToken);
        return response;
    }
}
