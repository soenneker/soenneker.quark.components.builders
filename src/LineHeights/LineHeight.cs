using Soenneker.Quark.Enums.Scales;
using Soenneker.Quark.Enums.Size;

namespace Soenneker.Quark.Components.Builders.LineHeights;

public static class LineHeight
{
    public static LineHeightBuilder L1 => new(Scale.S1.Value);
    public static LineHeightBuilder Sm => new(Size.Small.Value);
    public static LineHeightBuilder Base => new("base");
    public static LineHeightBuilder Lg => new(Size.Large.Value);
}
