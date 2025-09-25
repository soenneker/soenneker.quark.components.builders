using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark;

public static class FontStyle
{
    public static FontStyleBuilder Italic => new(Enums.FontStyles.FontStyle.ItalicValue);
    public static FontStyleBuilder Normal => new(Enums.FontStyles.FontStyle.NormalValue);
    public static FontStyleBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static FontStyleBuilder Initial => new(GlobalKeyword.InitialValue);
    public static FontStyleBuilder Revert => new(GlobalKeyword.RevertValue);
    public static FontStyleBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static FontStyleBuilder Unset => new(GlobalKeyword.UnsetValue);
}
