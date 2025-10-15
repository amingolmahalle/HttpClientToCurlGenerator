using HttpClientToCurl.Config;
using HttpClientToCurl.Extensions;

namespace HttpClientToCurl.HttpMessageHandlers;

public class CurlGeneratorHttpMessageHandler : DelegatingHandler
{
    private readonly GlobalConfig _config;

    public CurlGeneratorHttpMessageHandler(GlobalConfig config) => _config = config;

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_config.TurnOnAll)
        {
            var consoleConfig = new ConsoleConfig();
            _config.ShowOnConsole?.Invoke(consoleConfig);
            if (consoleConfig.TurnOn)
                HttpRequestMessageExtensions.GenerateCurlInConsole(request, null);

            var fileConfig = new FileConfig();
            _config.SaveToFile?.Invoke(fileConfig);
            if (fileConfig.TurnOn)
                HttpRequestMessageExtensions.GenerateCurlInFile(request, null, _config.SaveToFile);
        }

        var response = base.SendAsync(request, cancellationToken);
        return response;
    }
}
