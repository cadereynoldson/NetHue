namespace NetHue;

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
/// - effects
/// - timed effects
/// - powerup 
/// </summary>
[SimpleJsonConverter(typeof(HueLightSimpleJsonConverter))]
public record HueLight : HueResource {

    /// <summary>
    /// The state of the HueLight, contains values such as on, the current CIE color, etc. 
    /// </summary>
    public HueLightState State { get; set; } = default!; 

    /// <summary>
    /// The range of mired colors this HueLight can produce.
    /// </summary>
    public MiredColorRange MiredColorRange { get; init; } = default!;

    /// <summary>
    /// The color gamut of CIE colors this HueLight can produce. 
    /// </summary>
    public CieColorGamut CieColorGamut { get; init; } = default!; 

    /// <summary>
    /// Alert effects that the HueLight supports.
    /// </summary>
    public List<string> AlertActionValues { get; init; } = default!;
}