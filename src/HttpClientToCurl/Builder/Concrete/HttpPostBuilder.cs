using HttpClientToCurl.Builder.Concrete.Common;

namespace HttpClientToCurl.Builder.Concrete;

public sealed class HttpPostBuilder() : RequestBuilder(BodyMode.Always);
