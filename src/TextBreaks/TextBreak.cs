namespace Soenneker.Quark.Components.Builders.TextBreaks;

public static class TextBreak
{
    public static TextBreakBuilder Enable => new(true);
    public static TextBreakBuilder Disable => new(false);
}
