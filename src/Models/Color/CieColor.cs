namespace NetHue;

/// <summary>
/// Represents a color in CIE color space. 
/// For more information, see https://en.wikipedia.org/wiki/CIE_1931_color_space
/// </summary>
public record CieColor
{

    /// <summary>
    /// The x value of this color in CIE color space.
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 0 -> 1 </description>
    /// </item>
    /// </list>
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// The y value of this color in CIE color space.
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 0 -> 1 </description>
    /// </item>
    /// </list>
    /// </summary>
    public double Y { get; init; }
}