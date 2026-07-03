using Microsoft.Extensions.Options;

namespace HttpMessageHandlerTest.UnitTest.Fakes;

public class FakeOptionsMonitor<T>(T value) : IOptionsMonitor<T> where T : class
{
    public T CurrentValue { get; } = value;
    public T Get(string? name) => CurrentValue;
    public IDisposable OnChange(Action<T, string> listener) => new Disposable();
    private sealed class Disposable : IDisposable { public void Dispose() { } }
}