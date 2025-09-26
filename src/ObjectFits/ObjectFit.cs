
namespace Soenneker.Quark;

/// <summary>
/// Simplified object-fit utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class ObjectFit
{
    /// <summary>
    /// object-fit: contain.
    /// </summary>
    public static ObjectFitBuilder Contain => new(ObjectFit.ContainValue);

    /// <summary>
    /// object-fit: cover.
    /// </summary>
    public static ObjectFitBuilder Cover => new(ObjectFit.CoverValue);

    /// <summary>
    /// object-fit: fill.
    /// </summary>
    public static ObjectFitBuilder Fill => new(ObjectFit.FillValue);

    /// <summary>
    /// object-fit: scale-down.
    /// </summary>
    public static ObjectFitBuilder ScaleDown => new(ObjectFit.ScaleDownValue);

    /// <summary>
    /// object-fit: none.
    /// </summary>
    public static ObjectFitBuilder None => new(ObjectFit.NoneValue);

    public static ObjectFitBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static ObjectFitBuilder Initial => new(GlobalKeyword.InitialValue);
    public static ObjectFitBuilder Revert => new(GlobalKeyword.RevertValue);
    public static ObjectFitBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static ObjectFitBuilder Unset => new(GlobalKeyword.UnsetValue);
}
