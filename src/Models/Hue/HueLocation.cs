namespace NetHue;

/// <summary>
/// Class storing information on a "location" configured in the Hue bridge (room, zone, etc.)
/// </summary>
public abstract record HueLocation : HueResource
{
    /// <summary>
    /// Children of this location (services, lights, etc) 
    /// </summary>
    public List<HueResourceIdentifier> Children { get; set; } = default!;

    /// <summary>
    /// References all services aggregating control and state of children in the location
    /// </summary>
    public List<HueResourceIdentifier> Services { get; set; } = default!;
}