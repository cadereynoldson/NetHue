namespace Hue;

public class HueLightState 
{
    /// <summary>
    /// Indicates if this <see cref="HueLight"/> is on. 
    /// </summary>
    public bool On { get; set; }

    /// <summary>
    /// The brightness of this <see cref="HueLight"/>
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 1 -> 254 </description>
    /// </item>
    /// </list>
    /// </summary>
    public double Brightness { get; set; }

    /// <summary>
    /// The CIE color of this <see cref="HueLight"/>. 
    /// </summary>
    public CieColor Cie { get; set; } = default!; 

    /// <summary>
    /// The mired color temperature of this <see cref="HueLight"/>.
    /// </summary>
    public MiredColor ColorTemperature { get; set; } = default!;
}