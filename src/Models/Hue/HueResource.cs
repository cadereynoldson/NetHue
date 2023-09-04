namespace NetHue;

using JsonConversion;

/// <summary>
/// Class containing shared information between all types of Hue products. 
/// </summary>
[SimpleJsonConverter(typeof(HueRoomSimpleJsonConverter))]
public abstract record HueResource {

    /// <summary>
    /// The Id of this model, provided from the Hue API V2. 
    /// </summary>
    public string Id { get;  init; } = default!;

    /// <summary>
    /// The human readable name of this HueModel object.
    /// </summary>
    /// [JsonPropertyName()]
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// The archetype of this HueResource. 
    /// </summary>
    public string Archetype { get; set; } = default!;

    /// <summary>
    /// Other data associated with this object, examples being product type, product name, and so on. 
    /// </summary>
    public Dictionary<string, string>? MetaData { get; set; }

}