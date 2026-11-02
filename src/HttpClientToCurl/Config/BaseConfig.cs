namespace HttpClientToCurl.Config;

public class BaseConfig
{
    public bool TurnOn { get; set; } = true;
    public bool NeedAddDefaultHeaders { get; set; } = true;
    public bool EnableCompression { get; set; }
    public string[] RedactedHeaders { get; set; } = [];

    /// <summary>
    /// Copies the options shared by every sink. Add new <see cref="BaseConfig"/> options here
    /// so the automatic (handler) mode cannot forget them.
    /// </summary>
    internal void CopyCommonOptionsFrom(BaseConfig source)
    {
        TurnOn = true;
        NeedAddDefaultHeaders = source.NeedAddDefaultHeaders;
        EnableCompression = source.EnableCompression;
        RedactedHeaders = source.RedactedHeaders;
    }
}
