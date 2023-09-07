namespace NetHue;

public record HueScene : HueResource
{
    /// <summary>
    /// The actions associated with this scene. 
    /// </summary>
    public List<HueSceneAction> Actions { get; set; } = default!;

    /// <summary>
    /// The group associated with this scene. 
    /// </summary>
    public HueResourceIdentifier Group { get; set; } = default!;

    /// <summary>
    /// The speed of the dynamic palette for this scene. 
    /// </summary>
    public double Speed { get; set; } = default!;

    /// <summary>
    /// Indicates where to automatically start the dynamically on active recall.
    /// </summary>
    public bool AutoDynamic { get; set; } = default!;

    /// <summary>
    /// Indicates the status of this scene. 
    /// </summary>
    public string Status { get; set; } = default!;
}