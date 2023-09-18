using JsonConversion;

namespace NetHue;

/// <summary>
/// Record containing updated values of a HueLight.
/// </summary>
[SimpleJsonConverter(typeof(HueLightEventJsonConverter))]
public class HueLightEvent : HueResourceEvent
{
    /// <summary>
    /// Indicates if this <see cref="HueLight"/> is on. 
    /// </summary>
    public bool? On { get; set; }

    /// <summary>
    /// The brightness of this <see cref="HueLight"/>
    /// </summary>
    public double? Brightness { get; set; }

    /// <summary>
    /// The current mode this HueLight is operating in, normal or streaming. 
    /// </summary>
    public string? Mode { get; set; } = default!;

    /// <summary>
    /// The CIE color of this <see cref="HueLight"/>. 
    /// Will be null if a color has not been set on this light.
    /// </summary>
    public CieColor? Color { get; set; }

    /// <summary>
    /// The mired color temperature of this <see cref="HueLight"/>.
    /// </summary>
    public MiredColor? ColorTemperature { get; set; }

    /// <summary>
    /// TODO: dynamics updates
    /// </summary>
    public string? Dynamics { get; set; }

    /// <summary>
    /// TODO: Alert updates
    /// </summary>
    public string? Alert { get; set; }

    /// <summary>
    /// TODO: Signaling updates
    /// </summary>
    public string? Signaling { get; set; }

    /// <summary>
    /// TODO: Gradient updates
    /// </summary>
    public string? Gradient { get; set; }

    /// <summary>
    /// TODO: Effect updates
    /// </summary>
    public string? Effect { get; set; }

    /// <summary>
    /// TODO: TimedEffect updates
    /// </summary>
    public string? TimedEffect { get; set; }

    /// <summary>
    /// TODO: Powerup updates. 
    /// </summary>
    public string? Powerup { get; set; }


    /// <summary>
    /// Applies the attributes of this class to a HueResource of type HueLight.
    /// </summary>
    /// <param name="resource">The HueLight to apply the attributes of this class to.</param>
    public override void Apply(HueResource resource)
    {
        // Only apply these fields HueLights. 
        if (resource is HueLight light)
        {
            if (On.HasValue)
            {
                light.On = On.Value;
            }

            if (Brightness.HasValue)
            {
                light.Brightness = Brightness.Value;
            }

            if (Mode != null)
            {
                light.Mode = Mode;
            }

            if (Color != null)
            {
                light.Color = Color;
            }

            if (ColorTemperature != null)
            {
                light.ColorTemperature = ColorTemperature;
            }
        }
    }
}