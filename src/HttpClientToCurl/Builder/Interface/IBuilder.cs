using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Interface;

public interface IBuilder
{
    string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config);
    string CreateCurl(HttpRequestMessage httpRequestMessage, Uri baseAddress, BaseConfig config);
}
