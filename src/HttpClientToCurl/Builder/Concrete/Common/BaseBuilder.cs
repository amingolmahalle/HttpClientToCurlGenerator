using System.Text;
using HttpClientToCurl.Builder.Interface;
using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Concrete.Common;

public abstract class BaseBuilder : IBuilder
{
    protected readonly StringBuilder _stringBuilder = new();

    public abstract string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config);
}
