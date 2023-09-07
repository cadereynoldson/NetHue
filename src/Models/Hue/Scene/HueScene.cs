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
}