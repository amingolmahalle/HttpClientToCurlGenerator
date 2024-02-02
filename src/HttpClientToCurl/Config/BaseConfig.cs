namespace HttpClientToCurl.Config;

public class BaseConfig
{
    public BaseConfig()
    {
        TurnOn = true;
        NeedAddDefaultHeaders = true;
        EnableCompression = false;
    }

    public bool TurnOn { get; set; }
    public bool NeedAddDefaultHeaders { get; set; }
    public bool EnableCompression { get; set; }
}
