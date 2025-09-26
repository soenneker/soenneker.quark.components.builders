
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

public static class FontWeight
{
    public static FontWeightBuilder Light => new(Enums.FontWeight.LightValue);
    public static FontWeightBuilder Normal => new(Enums.FontWeight.NormalValue);
    public static FontWeightBuilder Medium => new(Enums.FontWeight.MediumValue);
    public static FontWeightBuilder Semibold => new(Enums.FontWeight.SemiboldValue);
    public static FontWeightBuilder Bold => new(Enums.FontWeight.BoldValue);
    public static FontWeightBuilder Bolder => new(Enums.FontWeight.BolderValue);
    public static FontWeightBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static FontWeightBuilder Initial => new(GlobalKeyword.InitialValue);
    public static FontWeightBuilder Revert => new(GlobalKeyword.RevertValue);
    public static FontWeightBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static FontWeightBuilder Unset => new(GlobalKeyword.UnsetValue);
}
