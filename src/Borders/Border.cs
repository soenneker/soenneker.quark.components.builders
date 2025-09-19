using Soenneker.Quark.Enums.Scales;

namespace Soenneker.Quark.Components.Builders.Borders;

/// <summary>
/// Simplified border utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Border
{
    /// <summary>
    /// No border (0).
    /// </summary>
    public static BorderBuilder S0 => new(Scale.S0.Value);

    /// <summary>
    /// Size 1 border (1px).
    /// </summary>
    public static BorderBuilder S1 => new(Scale.S1.Value);

    /// <summary>
    /// Size 2 border (2px).
    /// </summary>
    public static BorderBuilder S2 => new(Scale.S2.Value);

    /// <summary>
    /// Size 3 border (3px).
    /// </summary>
    public static BorderBuilder S3 => new(Scale.S3.Value);

    /// <summary>
    /// Size 4 border (4px).
    /// </summary>
    public static BorderBuilder S4 => new(Scale.S4.Value);

    /// <summary>
    /// Size 5 border (5px).
    /// </summary>
    public static BorderBuilder S5 => new(Scale.S5.Value);
}
