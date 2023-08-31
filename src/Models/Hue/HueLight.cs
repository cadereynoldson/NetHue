namespace Hue;

using JsonConversion;

/// <summary>
/// Class containing information of a Phillips Hue Light. Contains meta data on: 
/// <list type="bullet">
/// <item>
/// <description> "type": (str) Type of the light </description>
/// </item>
/// <item>
/// <description> "id_v1": (str) Hue API v1 id. (Optional) </description>
/// </item>
/// <item>
/// <description> 
///     "owner": (str) Information on the owner of the hue light,
///     will be a dict containing the keys "rid" and "rtype".
///     See https://developers.meethue.com/develop/hue-api-v2/api-reference/#resource_light_get
///     for descriptions on these values. 
///  </description>
/// </item>
/// <item>
/// <description> "archetype": (str) Light archetype </description>
/// </item>
/// <item>
/// <description> "fixed_mired": (int) A fixed mired value of the white lamp. </description>
/// </item>
/// <item>
/// <description> "min_dim_level": (int) Percentage of the maximum lumen the device outputs on minimum brightness. (0 -> 100) </description>
/// </item>
/// </list>
/// TODO list from https://developers.meethue.com/develop/hue-api-v2/api-reference/#resource_light_get
/// - dynamics
/// - alert
/// - signaling
/// - mode
/// - gradient
/// - effects
/// - timed effects
/// - powerup 
/// </summary>
[SimpleJsonConverter(typeof(HueLightSimpleJsonConverter))]
public class HueLight : HueResource {

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

    /// <summary>
    /// The range of mired colors this light can produce.
    /// </summary>
    public MiredColorRange MiredColorRange { get; init; } = default!;

    /// <summary>
    /// The color gamut of CIE colors this light can produce. 
    /// </summary>
    public CieColorGamut CieColorGamut { get; init; } = default!; 

}