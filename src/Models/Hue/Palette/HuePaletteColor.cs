namespace NetHue;

/// <summary>
/// Record containing information on a single color in a Palette. 
/// </summary>
public record HuePaletteColor : CieColor
{
    /// <summary>
    /// The brightness of this color in a palette. 
    /// </summary>
    public double Brightness { get; set; } = default!;
}