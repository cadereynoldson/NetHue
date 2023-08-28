/// <summary>
/// Class representing the color gamut of a CIE color space. 
/// For those not familiar with color gamuts, forms a triangle on the CIE color spectrum.
/// All colors inside said triangle will be recreateable by a device. 
/// For more information, see https://en.wikipedia.org/wiki/CIE_1931_color_space
/// </summary>
public class CieColorGamut {

    /// <summary>
    /// The red value of this color gamut.
    /// </summary>
    public CieColor Red { get; init; } = default!;

    /// <summary>
    /// The green value of this color gamut.
    /// </summary>
    public CieColor Green { get; init; } = default!;

    /// <summary>
    /// The blue value of this color gamut.
    /// </summary>
    public CieColor Blue { get; init; } = default!;
}