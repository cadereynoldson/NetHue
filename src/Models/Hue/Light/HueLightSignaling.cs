namespace NetHue;

/// <summary>
/// Record containing information on the signaling status of a HueLight. 
/// </summary>
public record HueLightSignaling
{
    
    /// <summary>
    /// Indicates which signal is currently active.
    /// </summary>
    public string Signal { get; set; } = default!;

    /// <summary>
    /// Timestamp indicating when the active signal is expected to end. Value is not set if there is no_signal
    /// </summary>
    public DateTime? EstimatedEnd { get; set; }

    /// <summary>
    /// Colors that were provided for the active effect
    /// </summary>
    public List<CieColor> Colors { get; set; } = default!;

    /// <summary>
    /// Signals that the HueLight supports.
    /// </summary>
    public List<string> SignalValues { get; init; } = default!; 

}