using System.Text;

namespace HttpClientToCurl.Builder.Concrete.Common;

public abstract class BaseBuilder
{
    protected readonly StringBuilder _stringBuilder = new();
}
