using JsonConversion;

namespace NetHue;

/// <summary>
/// Class representing a grouped hue 
/// </summary>
[SimpleJsonConverter(typeof(HueLightGroupSimpleJsonConverter))]
public record HueLightGroup : HueResource
{
    /// <summary>
    /// The owner of this grouped light. 
    /// </summary>
    public HueResourceIdentifier Owner { get; init; } = default!;

    /// <summary>
    /// Indicates if this grouped light is on. 
    /// </summary>
    public bool? On { get; set; } = default!;

    /// <summary>
    /// The brightness of this grouped light. 
    /// </summary>
    public double? Brightness { get; set; } = default!;

    /// <summary>
    /// Alert effects the grouped light supports. 
    /// </summary>
    public List<string>? AlertActionValues { get; init; } = default!;

    /// <summary>
    /// Signals that the grouped light supports. 
    /// </summary>
    public List<string>? SignalingValues { get; init; } = default!;
}