using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Interface;

public interface IBuilder
{
    string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config);
}
