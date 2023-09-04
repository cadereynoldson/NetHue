namespace NetHue;

/// <summary>
/// Record containing information the state of a HueLight. 
/// </summary>
public record HueLightState 
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
    /// The current mode this HueLight is operating in, normal or streaming. 
    /// </summary>
    public string Mode { get; set; } = default!;

    /// <summary>
    /// The CIE color of this <see cref="HueLight"/>. 
    /// </summary>
    public CieColor Cie { get; set; } = default!; 

    /// <summary>
    /// The mired color temperature of this <see cref="HueLight"/>.
    /// </summary>
    public MiredColor ColorTemperature { get; set; } = default!;

    /// <summary>
    /// The state of the dynamic effects of this HueLight.
    /// </summary>
    public HueLightDynamics Dynamics { get; set; } = default!;

    /// <summary>
    /// The state of an active signal. Will be non-null if device allows for, or has been configured for this.
    /// </summary>
    public HueLightSignaling? Signaling { get; set; } = default!;

    /// <summary>
    /// The state of a HueLight gradient. Will be non-null if device allows for, or has been configured for this.
    /// </summary>
    public HueLightGradient? Gradient { get; set; } = default!; 

    /// <summary>
    /// The state of a HueLight effect. Will be non-null if currently being used. 
    /// </summary>
    public HueLightEffect? Effect {get; set; } = default!;

    /// <summary>
    /// The state of a timed HueLight effect. Will be non-null if currently being used. 
    /// </summary>
    public HueLightTimedEffect? TimedEffect { get; set; } = default!;
}