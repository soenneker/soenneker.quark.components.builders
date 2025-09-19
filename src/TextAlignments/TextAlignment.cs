using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.Builders.TextAlignments;

public static class TextAlignment
{
	public static TextAlignmentBuilder Start => new(Enums.TextAlignments.TextAlignment.StartValue);

	public static TextAlignmentBuilder Center => new(Enums.TextAlignments.TextAlignment.CenterValue);

	public static TextAlignmentBuilder End => new(Enums.TextAlignments.TextAlignment.EndValue);

	public static TextAlignmentBuilder Inherit => new(GlobalKeyword.InheritValue);
	public static TextAlignmentBuilder Initial => new(GlobalKeyword.InitialValue);
	public static TextAlignmentBuilder Revert => new(GlobalKeyword.RevertValue);
	public static TextAlignmentBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
	public static TextAlignmentBuilder Unset => new(GlobalKeyword.UnsetValue);
}