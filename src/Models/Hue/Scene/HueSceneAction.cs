namespace NetHue;

/// <summary>
/// Represents an action to be executed.
/// </summary>
public record HueSceneAction
{
    /// <summary>
    /// The identifier of the light to execute the action on
    /// </summary>
    public HueResourceIdentifier Target { get; set; } = default!;

    /// <summary>
    /// Indicates if the resource should be turned on or off.
    /// </summary>
    public bool? On { get; set; }

    /// <summary>
    /// Indicates the brightness to apply to the resource. 
    /// </summary>
    public double? Brightness { get; set; }

    /// <summary>
    /// Indicates the CieColor to apply to the resource. 
    /// </summary>
    public CieColor? Color { get; set; }

    /// <summary>
    /// Indicates the ColorTemperature to apply to the resource to. 
    /// </summary>
    public MiredColor? ColorTemperature { get; set; }

    /// <summary>
    /// Indicates the gradient to apply to the resource. 
    /// </summary>
    public HueGradient? Gradient { get; set; }

    /// <summary>
    /// Indicates the effect to apply to the resource. 
    /// </summary>
    public string? Effect { get; set; }

    /// <summary>
    /// The duration of a light transition. 
    /// </summary>
    public int? DynamicDuration { get; set; }
    
}