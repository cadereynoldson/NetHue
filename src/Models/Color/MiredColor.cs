/// <summary>
/// Represents a mired color temperature.
/// Indicates the colour temperature shift provided by a filter for a light source.
/// </summary>
public record MiredColor
{

    /// <summary>
    /// Represents a mired color.
    /// <list type="bullet">
    /// <item>
    /// <description> Hue values: 153 -> 500 </description>
    /// </item>
    /// </list>
    /// </summary>    
    public int MiredValue { get; init; }

}