using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Quark.Components.Builders.Tests;

[Collection("Collection")]
public sealed class ComponentBuildersTests : FixturedUnitTest
{
    public ComponentBuildersTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {

    }

    [Fact]
    public void Default()
    {

    }

    [Fact]
    public void ColorBuilder_ThemeColors_GeneratesCorrectOutput()
    {
        // Test Bootstrap theme colors (should generate classes)
        ColorBuilder primary = Color.Primary;
        Assert.Equal("primary", primary.ToClass());
        Assert.Equal("", primary.ToStyle());
        Assert.Equal("primary", primary.ToString());

        ColorBuilder danger = Color.Danger;
        Assert.Equal("danger", danger.ToClass());
        Assert.Equal("", danger.ToStyle());
        Assert.Equal("danger", danger.ToString());
    }

    [Fact]
    public void ColorBuilder_CssKeywords_GeneratesCorrectOutput()
    {
        // Test CSS keywords (should generate styles)
        ColorBuilder inherit = Color.Inherit;
        Assert.Equal("", inherit.ToClass());
        Assert.Equal("inherit", inherit.ToStyle());
        Assert.Equal("inherit", inherit.ToString());

        ColorBuilder initial = Color.Initial;
        Assert.Equal("", initial.ToClass());
        Assert.Equal("initial", initial.ToStyle());
        Assert.Equal("initial", initial.ToString());
    }

    [Fact]
    public void ColorBuilder_ResponsiveColors_WorkCorrectly()
    {
        ColorBuilder responsiveColor = Color.Primary.OnTablet;
        string classResult = responsiveColor.ToClass();
        
        // Should contain both the color class and responsive breakpoint
        // Tablet breakpoint maps to "sm" in Bootstrap
        Assert.Contains("sm-primary", classResult);
        Assert.Contains("sm", classResult);
    }

    [Fact]
    public void ColorBuilder_CustomCssColors_WorkCorrectly()
    {
        // Test custom CSS color values
        ColorBuilder customColor = Color.FromCss("#ff0000");
        Assert.Equal("", customColor.ToClass());
        Assert.Equal("#ff0000", customColor.ToStyle());
        Assert.Equal("#ff0000", customColor.ToString());

        ColorBuilder customColor2 = Color.FromCss("rgb(255, 0, 0)");
        Assert.Equal("", customColor2.ToClass());
        Assert.Equal("rgb(255, 0, 0)", customColor2.ToStyle());
        Assert.Equal("rgb(255, 0, 0)", customColor2.ToString());
    }
}
