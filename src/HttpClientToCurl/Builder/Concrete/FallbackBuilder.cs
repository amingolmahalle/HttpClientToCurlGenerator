using HttpClientToCurl.Builder.Concrete.Common;

namespace HttpClientToCurl.Builder.Concrete;

/// <summary>Handles every HTTP method without a dedicated builder (HEAD, OPTIONS, TRACE, custom verbs).</summary>
internal sealed class FallbackBuilder() : RequestBuilder(BodyMode.WhenContentPresent);
