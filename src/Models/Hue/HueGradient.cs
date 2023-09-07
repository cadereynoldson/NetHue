namespace NetHue;

/// <summary>
/// Record containing information on a HueGradient. 
/// </summary>
public record HueGradient
{
    /// <summary>
    /// Collection of gradient points, max value of 5. 
    /// </summary>
    public List<CieColor> Points { get; set; } = default!;

    /// <summary>
    /// The mode in which the points are currently being displayed. 
    /// </summary>
    public string GradientMode { get; set; } = default!;

}