using Microsoft.Extensions.Logging;

namespace HttpClientToCurl.Config;

public class LoggerConfig : BaseConfig
{
    public LogLevel LogLevel { get; set; } = LogLevel.Debug;
}
