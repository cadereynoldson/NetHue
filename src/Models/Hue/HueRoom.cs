namespace NetHue;

/// <summary>
/// Record containing data on a configured "room"
/// </summary>
public record HueRoom : HueResource
{

    /// <summary>
    /// Children of this room (services, lights, etc) 
    /// </summary>
    public List<HueResourceIdentifier> Children { get; set; } = default!;

    /// <summary>
    /// References all services aggregating control and state of children in the group
    /// </summary>
    public List<HueResourceIdentifier> Services { get; set; } = default!;

}