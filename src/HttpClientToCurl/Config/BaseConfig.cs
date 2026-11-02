namespace HttpClientToCurl.Config;

public class BaseConfig
{
    public bool TurnOn { get; set; } = true;
    /// <summary>
    /// Include <c>HttpClient.DefaultRequestHeaders</c> in the generated curl. <c>Default is true</c>.
    /// <para>
    /// Only applies to the <see cref="HttpClient"/> extension methods. In automatic mode (the DI handler)
    /// <see cref="HttpClient"/> has already merged its default headers into the request before the handler runs,
    /// so they are always included and this setting has no effect.
    /// </para>
    /// </summary>
    public bool NeedAddDefaultHeaders { get; set; } = true;
    /// <summary>Append <c>--compressed</c> to the generated curl. <c>Default is false</c>.</summary>
    public bool EnableCompression { get; set; }
    /// <summary>
    /// Header names (case-insensitive) whose values are replaced with <c>[REDACTED]</c>.
    /// Applies to default, request and content headers, in both manual and automatic mode.
    /// </summary>
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
