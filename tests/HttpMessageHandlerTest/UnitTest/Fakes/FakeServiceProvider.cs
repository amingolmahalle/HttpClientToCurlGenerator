using HttpClientToCurl.HttpMessageHandlers;

namespace HttpMessageHandlerTest.UnitTest.Fakes;

public class FakeServiceProvider(object service) : IServiceProvider
{
    private readonly object _service = service;

    public object GetService(Type serviceType) => serviceType == typeof(CurlGeneratorHttpMessageHandler) ? _service : null;
}