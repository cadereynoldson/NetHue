namespace NetHue; 

/// <summary>
/// Record containing information on the dynamic state of a HueLight. 
/// </summary>
public record HueLightDynamics {

    /// <summary>
    /// Current status of the lamp with dynamics
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Speed in ms of dynamic palette or effect. 
    /// </summary>
    public double? Speed { get; set; }

    /// <summary>
    /// Indicates if the value in speed is valid. 
    /// </summary>
    public bool SpeedValid { get; set; }

    /// <summary>
    /// Statuses in which a HueLight could be when playing dynamics
    /// </summary>
    public List<string> DynamicStatusValues { get; init; } = default!;
}