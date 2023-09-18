namespace NetHue;

/// <summary>
/// Class containing shared information between all types of Hue products. 
/// </summary>
public abstract record HueResource
{

    /// <summary>
    /// The Id of this model, provided from the Hue API V2. 
    /// </summary>
    public string Id { get; init; } = default!;

    /// <summary>
    /// The human readable name of this HueModel object.
    /// </summary>
    public string? Name { get; set; } = default!;

    /// <summary>
    /// The archetype of this HueResource. 
    /// </summary>
    public string? Archetype { get; set; } = default!;
}