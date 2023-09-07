namespace NetHue;

/// <summary>
/// Class representing a group of colors that describle the palette of colors to used when playing dynamics. 
/// </summary>
public record HueScenePalette
{
    /// <summary>
    /// The colors of the palette. List of length 0 to 9. 
    /// </summary>
    public List<HuePaletteColor> Colors { get; set; } = default!;

    /// <summary>
    /// The dimming level of the palette. Should be of length 0 or 1. 
    /// API returns a list, store as a list for future release compatibility. 
    /// </summary>
    public List<double> Dimming { get; set; } = default!;

    /// <summary>
    /// The color temperature of the palette. Should be of length 0 or 1. 
    /// API returns a list, store as a list for future release compatibility.
    /// </summary>
    public List<HuePaletteColorTemperature> ColorTemperature { get; set; } = default!;

    /// <summary>
    /// The effects of the palette. List of length 0 to 3. 
    /// </summary>
    public List<string> Effects { get; set; } = default!;
}