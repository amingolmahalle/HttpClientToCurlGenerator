namespace HttpClientToCurl.Config;

public class BaseConfig
{
    public bool TurnOn { get; set; } = true;
    public bool NeedAddDefaultHeaders { get; set; } = true;
    public bool EnableCompression { get; set; } = false;
}
