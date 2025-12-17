using Microsoft.Extensions.Logging;

namespace HttpClientToCurl.Config;

public sealed class CompositConfig
{
    /// <summary>
    /// Set true to create curl output; false to disable it.  <c>Default is true</c>. 
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// Set true to show curl on the console; false to disable it. <c>Default is true</c>.
    /// <para>If Enable is set to false, it will be ignored.</para>
    /// </summary>
    public ConsoleConfig ShowOnConsole { get; set; }

    /// <summary>
    /// Set true to save the curl file; false to disable it. <c>Default is true</c>.
    /// <para>If Enable is set to false, it will be ignored.</para>
    /// </summary>
    public FileConfig SaveToFile { get; set; }

    /// <summary>
    /// Set true to send curl to <see cref="ILogger"/>; false to disable it. <c>Default is false</c>.
    /// <para>If Enable is set to false, it will be ignored.</para>
    /// </summary>
    public LoggerConfig SendToLogger { get; set; }
}
