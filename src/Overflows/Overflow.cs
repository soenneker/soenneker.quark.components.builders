
namespace Soenneker.Quark;

/// <summary>
/// Simplified overflow utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Overflow
{
    /// <summary>
    /// Overflow auto.
    /// </summary>
    public static OverflowBuilder Auto => new(Overflow.AutoValue);

    /// <summary>
    /// Overflow hidden.
    /// </summary>
    public static OverflowBuilder Hidden => new(Overflow.HiddenValue);

    /// <summary>
    /// Overflow visible.
    /// </summary>
    public static OverflowBuilder Visible => new(Overflow.VisibleValue);

    /// <summary>
    /// Overflow scroll.
    /// </summary>
    public static OverflowBuilder Scroll => new(Overflow.ScrollValue);

    public static OverflowBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static OverflowBuilder Initial => new(GlobalKeyword.InitialValue);
    public static OverflowBuilder Revert => new(GlobalKeyword.RevertValue);
    public static OverflowBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static OverflowBuilder Unset => new(GlobalKeyword.UnsetValue);
}
