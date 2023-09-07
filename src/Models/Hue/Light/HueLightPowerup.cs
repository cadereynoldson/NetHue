namespace NetHue;

/// <summary>
/// Record containing information on the state of a HueLight powerup operation. (What happens when you turn the light on)
/// </summary>
public record HueLightPowerup
{

    /// <summary>
    /// What preset happens when you turn on a light. 
    /// </summary>
    public PowerupPreset Preset { get; set; } = default!;

    /// <summary>
    /// Indicates if the shown values have been configured in the HueLight.
    /// </summary>
    public bool Configured { get; set; } = false;

    /// <summary>
    /// Indicates if the light is on or off. 
    /// </summary>
    public bool On { get; set; } = default!;

    /// <summary>
    /// Indicates what will happen when the light is powered on. 
    /// </summary>
    public OnMode OnBehavior { get; set; } = default!;

    /// <summary>
    /// The brightness to set the light to on power up. 
    /// </summary>
    public double? Brightness { get; set; }

    /// <summary>
    /// The on startup dimming behavior of the lamp. 
    /// </summary>
    public DimmingMode? BrightnessBehavior { get; set; } = default!;

    /// <summary>
    /// The on startup color behavior of the lamp. 
    /// </summary>
    public ColorMode? ColorBehavior { get; set; } = default!;

    /// <summary>
    /// The MiredColor to set the HueLight to on startup. 
    /// </summary>
    public MiredColor? ColorTemperature { get; set; } = default!;

    /// <summary>
    /// The CieColor to set the HueLight to on startup. 
    /// </summary>
    public CieColor? CieColor { get; set; } = default!;

    /// <summary>
    /// Enum containing information on presets for power up operations.
    /// </summary>
    public enum PowerupPreset
    {
        /// <summary>
        /// Safety preset. 
        /// </summary>
        SAFETY,

        /// <summary>
        /// Powerfail preset. 
        /// </summary>
        POWERFAIL,

        /// <summary>
        /// Behavior indicating light will take previous state it had before being turned off. 
        /// </summary>
        LAST_ON_STATE,

        /// <summary>
        /// Custom preset, allows for custom config of power on behavior. 
        /// </summary>
        CUSTOM
    }

    /// <summary>
    /// Enum containing definitions for what will happen when a light is turned on or off. 
    /// </summary>
    public enum OnMode
    {
        /// <summary>
        /// Default behavior, indicates the light will just be turned on when turned on. 
        /// </summary>
        ON,

        /// <summary>
        /// Alternates between on and off on each subsequet power toggle. 
        /// </summary>
        TOGGLE,

        /// <summary>
        /// Returns to the state the light was in before powering off. 
        /// </summary>
        PREVIOUS
    }

    /// <summary>
    /// Enum containing definitions on what dimming will occur on power up of a HueLight. 
    /// </summary>
    public enum DimmingMode
    {
        /// <summary>
        /// Will set the dimming level to the specified value in dimming on power up. 
        /// </summary>
        DIMMING,

        /// <summary>
        /// Will set the light to the previous brightness level it was at prior to power on. 
        /// </summary>
        PREVIOUS
    }

    /// <summary>
    /// Enum defining what color mode the light should take on powerup. 
    /// </summary>
    public enum ColorMode
    {
        /// <summary>
        /// Sets the HueLight to the MiredColor on startup. 
        /// Note, some lights may not support this. 
        /// </summary>
        MIRED,

        /// <summary>
        /// Sets the HueLight to the CIE color on startup.
        /// Note, some lights may not support this. 
        /// </summary>
        CIE,

        /// <summary>
        /// Sets the HueLight to the color it was prior to turning off on startup. 
        /// </summary>
        PREVIOUS
    }
}