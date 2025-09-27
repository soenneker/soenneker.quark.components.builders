using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

public static class BoxShadow
{
    public static BoxShadowBuilder None => new(Enums.BoxShadowKeyword.NoneValue);
    public static BoxShadowBuilder Base => new("base");
    public static BoxShadowBuilder Sm => new(SizeType.Small.Value);
    public static BoxShadowBuilder Lg => new(SizeType.Large.Value);
}