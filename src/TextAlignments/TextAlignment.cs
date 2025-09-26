
namespace Soenneker.Quark;

public static class TextAlignment
{
	public static TextAlignmentBuilder Start => new(TextAlignment.StartValue);

	public static TextAlignmentBuilder Center => new(TextAlignment.CenterValue);

	public static TextAlignmentBuilder End => new(TextAlignment.EndValue);

	public static TextAlignmentBuilder Inherit => new(GlobalKeyword.InheritValue);
	public static TextAlignmentBuilder Initial => new(GlobalKeyword.InitialValue);
	public static TextAlignmentBuilder Revert => new(GlobalKeyword.RevertValue);
	public static TextAlignmentBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
	public static TextAlignmentBuilder Unset => new(GlobalKeyword.UnsetValue);
}
