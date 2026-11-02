namespace HttpMessageHandlerTest.UnitTest;

/// <summary>
/// Tests that redirect the global <see cref="Console.Out"/> must not run in parallel with each other.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public class ConsoleCollection
{
    public const string Name = "Console";
}
