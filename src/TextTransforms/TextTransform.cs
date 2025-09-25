using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark;

public static class TextTransform
{
    public static TextTransformBuilder Lowercase => new(Enums.TextTransforms.TextTransform.LowercaseValue);
    public static TextTransformBuilder Uppercase => new(Enums.TextTransforms.TextTransform.UppercaseValue);
    public static TextTransformBuilder Capitalize => new(Enums.TextTransforms.TextTransform.CapitalizeValue);

    public static TextTransformBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static TextTransformBuilder Initial => new(GlobalKeyword.InitialValue);
    public static TextTransformBuilder Revert => new(GlobalKeyword.RevertValue);
    public static TextTransformBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static TextTransformBuilder Unset => new(GlobalKeyword.UnsetValue);
}
