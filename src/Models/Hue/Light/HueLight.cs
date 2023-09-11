namespace NetHue;

using JsonConversion;

/// <summary>
/// Record containing information on a HueLight. 
/// See <a href="https://developers.meethue.com/develop/hue-api-v2/api-reference/#resource_light_get">hue light GET documentation</a>
/// for more information.
/// </summary>
[SimpleJsonConverter(typeof(HueLightSimpleJsonConverter))]
public record HueLight : HueResource
{

    /// <summary>
    /// The owner of the HueLight. Owner in a sense of the bridge API.
    /// </summary>
    public HueResourceIdentifier Owner { get; set; } = default!;

    /// <summary>
    /// Indicates if this <see cref="HueLight"/> is on. 
    /// </summary>
    public bool On { get; set; }

    /// <summary>
    /// The brightness of this <see cref="HueLight"/>
    /// </summary>
    public double? Brightness { get; set; }

    /// <summary>
    /// The current mode this HueLight is operating in, normal or streaming. 
    /// </summary>
    public string Mode { get; set; } = default!;

    /// <summary>
    /// The CIE color of this <see cref="HueLight"/>. 
    /// Will be null if a color has not been set on this light.
    /// </summary>
    public CieColor? Color { get; set; }

    /// <summary>
    /// The color gamut of CIE colors this HueLight can produce. 
    /// Some lights don't return this information - in this case, fetch color 
    /// gamut information from CieColorGamut.FromLight();
    /// </summary>
    public CieColorGamut? CieColorGamut { get; init; }

    /// <summary>
    /// One of the Gamut types supported by Hue. Value of A, B, or C.
    /// </summary>
    public char CieColorGamutType { get; init; }

    /// <summary>
    /// The mired color temperature of this <see cref="HueLight"/>.
    /// </summary>
    public MiredColor? ColorTemperature { get; set; }

    /// <summary>
    /// The range of mired colors this HueLight can produce.
    /// </summary>
    public MiredColorRange? MiredColorRange { get; init; }

    /// <summary>
    /// The state of the dynamic effects of this HueLight.
    /// </summary>
    public HueLightDynamics? Dynamics { get; set; }

    /// <summary>
    /// Alert effects that the HueLight supports.
    /// </summary>
    public List<string> AlertActionValues { get; init; } = default!;

    /// <summary>
    /// The state of an active signal. Will be non-null if device allows for, or has been configured for this.
    /// </summary>
    public HueLightSignaling? Signaling { get; set; }

    /// <summary>
    /// The state of a HueLight gradient. Will be non-null if device allows for, or has been configured for this.
    /// </summary>
    public HueLightGradient? Gradient { get; set; }

    /// <summary>
    /// The state of a HueLight effect. Will be non-null if currently being used. 
    /// </summary>
    public HueLightEffect? Effect { get; set; }

    /// <summary>
    /// The state of a timed HueLight effect. Will be non-null if currently being used. 
    /// </summary>
    public HueLightTimedEffect? TimedEffect { get; set; }

    /// <summary>
    /// The state of what will happen to a HueLight on powerup. Will be non-null if currently being used. 
    /// </summary>
    public HueLightPowerup? Powerup { get; set; }
}