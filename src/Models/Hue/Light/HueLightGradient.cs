namespace NetHue;

/// <summary>
/// Record containing information on the state of a HueLight's gradient. 
/// </summary>
public record HueLightGradient
{
    /// <summary>
    /// Collection of gradient points, max value of 5. 
    /// </summary>
    public List<CieColor> Points { get; set; } = default!;

    /// <summary>
    /// The mode in which the points are currently being displayed. 
    /// </summary>
    public string GradientMode { get; set; } = default!;

    /// <summary>
    /// The number of color points that a gradient HueLight is capable of showing with gradience. 
    /// </summary>
    public int GradientPointsCapable { get; set; }

    /// <summary>
    /// Modes a gradient device can deploy the gradient palette of colors
    /// </summary>
    public List<string>? GradientModeValues { get; set; }

    /// <summary>
    /// The number of pixels in a gradient device. 
    /// </summary>
    public int GradientPixelCount { get; set; }
}