using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Director;

public class Creator
{
    private readonly Interface.IBuilder _builder;

    public Creator(Interface.IBuilder builder)
    {
        _builder = builder;
    }

    public string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        return _builder.Create(httpClient, httpRequestMessage, config);
    }
}
