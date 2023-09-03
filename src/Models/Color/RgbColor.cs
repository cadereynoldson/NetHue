using Hue;

public class RgbColor {

    /// <summary>
    /// The R value of this color.
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 1 -> 255 </description>
    /// </item>
    /// </list>
    /// </summary>
    public int R { get; init; }

    /// <summary>
    /// The G value of this color.
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 1 -> 255 </description>
    /// </item>
    /// </list>
    /// </summary>
    public int G { get; init; }

    /// <summary>
    /// The R value of this color.
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 1 -> 255 </description>
    /// </item>
    /// </list>
    /// </summary>
    public int B { get; init; }

    public RgbColor(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    /// Converts this RGB color to the closest CIE color in a given gamut. 
    /// </summary>
    /// <param name="gamut">The gamut of the CIE color to convert.</param>
    /// <returns></returns>
    public CieColor ToCie(CieColorGamut gamut)
    {  
        return ColorConverter.CieFromRgb(this, gamut);
    }

    /// <summary>
    /// Generates a random RGB color.
    /// </summary>
    /// <returns></returns>
    public static RgbColor Random()
    {
        var random = new Random();
        return new RgbColor(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
    }
}