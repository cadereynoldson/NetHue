namespace NetHue;

public class RgbColor
{

    /// <summary>
    /// RGB preset color: Red
    /// </summary>
    public static RgbColor Red { get; } = new RgbColor(255, 0, 0);

    // <summary>
    /// RGB preset color: Green
    /// </summary>
    public static RgbColor Green { get; } = new RgbColor(0, 255, 0);

    /// <summary>
    /// RGB preset color: Blue
    /// </summary>
    public static RgbColor Blue { get; } = new RgbColor(0, 0, 255);

    /// <summary>
    /// RGB preset color: White
    /// </summary>
    public static RgbColor White { get; } = new RgbColor(255, 255, 255);

    /// <summary>
    /// RGB preset color: Yellow
    /// </summary>
    public static RgbColor Yellow { get; } = new RgbColor(255, 255, 0);

    /// <summary>
    /// RGB preset color: Purple
    /// </summary>
    public static RgbColor Purple { get; } = new RgbColor(128, 0, 128);

    /// <summary>
    /// RGB preset color: Orange
    /// </summary>
    public static RgbColor Orange { get; } = new RgbColor(255, 165, 0);

    /// <summary>
    /// RGB preset color: Pink
    /// </summary>
    public static RgbColor Pink { get; } = new RgbColor(255, 192, 203);

    /// <summary>
    /// RGB preset color: Brown
    /// </summary>
    public static RgbColor Brown { get; } = new RgbColor(165, 42, 42);

    /// <summary>
    /// RGB preset color: Cyan
    /// </summary>
    public static RgbColor Cyan { get; } = new RgbColor(0, 255, 255);

    /// <summary>
    /// RGB preset color: Lavendar
    /// </summary>
    public static RgbColor Lavender { get; } = new RgbColor(230, 230, 250);

    /// <summary>
    /// RGB preset color: Gold
    /// </summary>
    public static RgbColor Gold { get; } = new RgbColor(255, 215, 0);

    /// <summary>
    /// RGB preset color: Silver
    /// </summary>
    public static RgbColor Silver { get; } = new RgbColor(192, 192, 192);

    /// <summary>
    /// RGB preset color: Maroon
    /// </summary>
    public static RgbColor Maroon { get; } = new RgbColor(128, 0, 0);

    /// <summary>
    /// RGB preset color: Teal
    /// </summary>
    public static RgbColor Teal { get; } = new RgbColor(0, 128, 128);

    /// <summary>
    /// RGB preset color: Olive
    /// </summary>
    public static RgbColor Olive { get; } = new RgbColor(128, 128, 0);

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

    /// <summary>
    /// Creates a new instance of a RgbColor. 
    /// </summary>
    /// <param name="r">The red value of this color.</param>
    /// <param name="g">The green value of this color.</param>
    /// <param name="b">The blue value of this color.</param>
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