using Microsoft.Extensions.Logging;

namespace HttpMessageHandlerTest.UnitTest.Fakes;

public class FakeLogger<T> : ILogger<T>
{
    public List<(LogLevel Level, string Message)> LoggedMessages { get; } = [];

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        LoggedMessages.Add((logLevel, message));
    }
}
