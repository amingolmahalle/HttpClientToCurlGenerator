using HttpClientToCurl.Builder.Concrete.Common;

namespace HttpClientToCurl.Builder.Concrete;

public sealed class HttpPatchBuilder() : RequestBuilder(BodyMode.Always);
