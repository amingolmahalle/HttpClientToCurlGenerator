using Microsoft.Extensions.DependencyInjection;

namespace HttpMessageHandlerTest.UnitTest.Builders;

public class HttpMessageHandlerBuilder : Microsoft.Extensions.Http.HttpMessageHandlerBuilder
{
    private readonly IList<DelegatingHandler> _additional = [];
    public override string Name { get; set; }
    public override HttpMessageHandler PrimaryHandler { get; set; }
    public override IList<DelegatingHandler> AdditionalHandlers => _additional;
    public override IServiceProvider Services => new ServiceCollection().BuildServiceProvider();
    public override HttpMessageHandler Build() => PrimaryHandler;

    public HttpMessageHandlerBuilder()
    {
        Name = "test";
        PrimaryHandler = new HttpClientHandler();
    }
}