using Soenneker.Quark.Enums.Scales;

namespace Soenneker.Quark;

/// <summary>
/// Simplified padding utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Padding
{
    /// <summary>
    /// No padding (0).
    /// </summary>
    public static PaddingBuilder S0 => new(Scale.S0Value);

    /// <summary>
    /// Size 1 padding (0.25rem).
    /// </summary>
    public static PaddingBuilder S1 => new(Scale.S1Value);

    /// <summary>
    /// Size 2 padding (0.5rem).
    /// </summary>
    public static PaddingBuilder S2 => new(Scale.S2Value);

    /// <summary>
    /// Size 3 padding (1rem).
    /// </summary>
    public static PaddingBuilder S3 => new(Scale.S3Value);

    /// <summary>
    /// Size 4 padding (1.5rem).
    /// </summary>
    public static PaddingBuilder S4 => new(Scale.S4Value);

    /// <summary>
    /// Size 5 padding (3rem).
    /// </summary>
    public static PaddingBuilder S5 => new(Scale.S5Value);
}
