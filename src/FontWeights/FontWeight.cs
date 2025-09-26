
namespace Soenneker.Quark;

public static class FontWeight
{
    public static FontWeightBuilder Light => new(FontWeight.LightValue);
    public static FontWeightBuilder Normal => new(FontWeight.NormalValue);
    public static FontWeightBuilder Medium => new(FontWeight.MediumValue);
    public static FontWeightBuilder Semibold => new(FontWeight.SemiboldValue);
    public static FontWeightBuilder Bold => new(FontWeight.BoldValue);
    public static FontWeightBuilder Bolder => new(FontWeight.BolderValue);
    public static FontWeightBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static FontWeightBuilder Initial => new(GlobalKeyword.InitialValue);
    public static FontWeightBuilder Revert => new(GlobalKeyword.RevertValue);
    public static FontWeightBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static FontWeightBuilder Unset => new(GlobalKeyword.UnsetValue);
}
