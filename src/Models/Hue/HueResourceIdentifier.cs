namespace NetHue; 

/// <summary>
/// Record storing basic information on a HueResource. Mainly used to reference resources from different resource types.
/// </summary>
public record HueResourceIdentifier
{
    /// <summary>
    /// The unique ID of the referenced resource. 
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// The type of resource this is. 
    /// </summary>
    public string ResourceType { get; set; } = default!;
}