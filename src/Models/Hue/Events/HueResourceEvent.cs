namespace NetHue;

/// <summary>
/// Abstract class which contains information on an event which occurs for a specific Hue device.
/// </summary>
public abstract class HueResourceEvent
{

    /// <summary>
    /// The ID of the resource which was effected.
    /// </summary>
    public string ResourceId { get; set; } = default!;

    /// <summary>
    /// Applies the non-null attribues of this class to the parameterized HueResource.
    /// HueResource should be of the type this event is configured to handle.
    /// </summary>
    /// <param name="resource">The resource to apply the attributes of this class to.</param>
    public abstract void Apply(HueResource resource);

}