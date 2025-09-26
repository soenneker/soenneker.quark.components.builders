using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark;

/// <summary>
/// Simplified overflow utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Overflow
{
    /// <summary>
    /// Overflow auto.
    /// </summary>
    public static OverflowBuilder Auto => new(Enums.Overflows.Overflow.AutoValue);

    /// <summary>
    /// Overflow hidden.
    /// </summary>
    public static OverflowBuilder Hidden => new(Enums.Overflows.Overflow.HiddenValue);

    /// <summary>
    /// Overflow visible.
    /// </summary>
    public static OverflowBuilder Visible => new(Enums.Overflows.Overflow.VisibleValue);

    /// <summary>
    /// Overflow scroll.
    /// </summary>
    public static OverflowBuilder Scroll => new(Enums.Overflows.Overflow.ScrollValue);

    public static OverflowBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static OverflowBuilder Initial => new(GlobalKeyword.InitialValue);
    public static OverflowBuilder Revert => new(GlobalKeyword.RevertValue);
    public static OverflowBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static OverflowBuilder Unset => new(GlobalKeyword.UnsetValue);
}
