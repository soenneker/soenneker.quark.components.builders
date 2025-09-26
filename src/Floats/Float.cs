
namespace Soenneker.Quark;

public static class Float
{
    public static FloatBuilder None => new(Float.NoneValue);

    public static FloatBuilder Left => new(Float.LeftValue);

    public static FloatBuilder Start => new(Float.InlineStartValue);

    public static FloatBuilder Right => new(Float.RightValue);

    public static FloatBuilder End => new(Float.InlineEndValue);

    public static FloatBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static FloatBuilder Initial => new(GlobalKeyword.InitialValue);
    public static FloatBuilder Revert => new(GlobalKeyword.RevertValue);
    public static FloatBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static FloatBuilder Unset => new(GlobalKeyword.UnsetValue);
}
