using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.Builders.BoxShadows;

public static class BoxShadow
{
    public static BoxShadowBuilder None => new(Enums.BoxShadows.BoxShadow.NoneValue);
    public static BoxShadowBuilder Base => new("base");
    public static BoxShadowBuilder Sm => new(Size.Small.Value);
    public static BoxShadowBuilder Lg => new(Size.Large.Value);
}
