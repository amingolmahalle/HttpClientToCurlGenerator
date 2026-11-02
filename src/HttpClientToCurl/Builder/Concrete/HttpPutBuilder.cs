using HttpClientToCurl.Builder.Concrete.Common;

namespace HttpClientToCurl.Builder.Concrete;

public sealed class HttpPutBuilder() : RequestBuilder(BodyMode.Always);
