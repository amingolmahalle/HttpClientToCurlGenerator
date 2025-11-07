using HttpClientToCurl.Config;

namespace HttpMessageHandlerTest.UnitTest.Builders;

public class CompositConfigBuilder
{
    private readonly CompositConfig _config;
    public CompositConfigBuilder()
    {
        _config = new CompositConfig();
    }

    public CompositConfigBuilder SetTurnOnAll(bool turnOnAll)
    {
        _config.TurnOnAll = turnOnAll;
        return this;
    }

    public CompositConfigBuilder SetShowOnConsole(ConsoleConfig? consoleConfig)
    {
        _config.ShowOnConsole = consoleConfig;
        return this;
    }

    public CompositConfigBuilder SetSaveToFile(FileConfig? fileConfig)
    {
        _config.SaveToFile = fileConfig;
        return this;
    }

    public CompositConfig Build()
    {
        return _config;
    }
}
