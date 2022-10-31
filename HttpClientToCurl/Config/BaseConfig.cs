namespace HttpClientToCurl.Config;

public abstract class BaseConfig
{
    public bool NeedAddDefaultHeaders { get; set; } = true;
    public bool TurnOn { get; set; } = true;
}