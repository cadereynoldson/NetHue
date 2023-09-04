namespace NetHue;

/// <summary>
/// Record containing information on an effect of a HueLight. 
/// </summary>
public record HueLightEffect
{
    /// <summary>
    /// The current effect of this HueLight. 
    /// </summary>
    public string Effect { get; set; } = default!;

    /// <summary>
    /// The current status value the HueLight is in while playing an effect.
    /// </summary>
    public string Status { get; set; } = default!;

    /// <summary>
    /// Possible effect values that can be set for this light. 
    /// </summary>
    public List<string> EffectValues { get; set; } = default!;

    /// <summary>
    /// Possible status values that this HueLight can be in when playing an effect. 
    /// </summary>
    public List<string> StatusValues { get; set;} = default!;

}