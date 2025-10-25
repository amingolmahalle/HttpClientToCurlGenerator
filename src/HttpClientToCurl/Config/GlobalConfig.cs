using HttpClientToCurl.Config.Others;

namespace HttpClientToCurl.Config;

public sealed class GlobalConfig
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0032:Use auto property", Justification = "<Pending>")]
    private ShowMode _showMode;

    public ShowMode ShowMode
    {
        get => _showMode;
        set
        {
            if (value is not ShowMode.None && value.HasFlag(ShowMode.None))
            {
                throw new ArgumentException("ShowMode.None cannot be combined with other flags.");
            }
            _showMode = value;
        }
    }
    public bool NeedAddDefaultHeaders { get; set; } = true;
    public bool ConsoleEnableCodeBeautification { get; set; } = false;
    public bool ConsoleEnableCompression { get; set; } = false;
    public string FileConfigFileName { get; set; }
    public string FileConfigPath { get; set; }
}
