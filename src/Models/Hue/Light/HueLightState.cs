namespace Hue;

// <summary>
/// Class which represents the current state of a HueLight. 
/// </summary>
class HueLightState {

    /// <summary>
    /// Indicates if the <see cref="HueLight"/> is on. 
    /// </summary>
    public bool On { get; set; }

    /// <summary>
    /// The brightness of the <see cref="HueLight"/>
    /// <list type="bullet">
    /// <item>
    /// <description> Values: 1 -> 254 </description>
    /// </item>
    /// </list>
    /// </summary>
    public int Brightness { get; set; }

    /// <summary>
    /// The CIE color of this light. 
    /// </summary>
    public CieColor Cie { get; set; } = default!; 

    /// <summary>
    /// The mired color temperature of the light.
    /// </summary>
    public MiredColor ColorTemperature { get; set; } = default!;

    /// <summary>
    /// Indicates the color mode in which the light is working. Values: 
    /// <list type="bullet">
    /// <item>
    /// <description> “hs” – The light is not performing an alert effect. </description>
    /// </item>
    /// <item>
    /// <description> “lselect” – The light is performing breathe cycles for 15 seconds or until an "alert": 
    /// "none" command is received. Note that this contains the last alert sent to the light and not its current state. 
    /// i.e. After the breathe cycle has finished the bridge does not reset the alert to “none“. 
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    public string ColorMode { get; set; } = default!;

}