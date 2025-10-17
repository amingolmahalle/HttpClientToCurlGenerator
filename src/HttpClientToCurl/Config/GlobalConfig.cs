namespace HttpClientToCurl.Config;

public sealed class GlobalConfig
{
    /// <summary>
    /// Set true to create curl output; false to disable it.  <c>Default is true</c>. 
    /// </summary>
    public bool TurnOnAll { get; set; } = true;

    /// <summary>
    /// Set true to show curl on the console; false to disable it. <c>Default is true</c>.
    /// <para>If TurnOnAll is set to false, it will be ignored.</para>
    /// </summary>
    public Action<ConsoleConfig> ShowOnConsole { get; set; }

    /// <summary>
    /// Set true to save the curl file; false to disable it. <c>Default is true</c>.
    /// <para>If TurnOnAll is set to false, it will be ignored.</para>
    /// </summary>
    public Action<FileConfig> SaveToFile { get; set; }
}
