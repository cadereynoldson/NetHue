namespace NetHue;

/// <summary>
/// Class containing information 
/// </summary>
public class HueLightGroupEvent : HueResourceEvent
{

    /// <summary>
    /// Indicates if this <see cref="HueLightGroup"/> is on. 
    /// </summary>
    public bool? On { get; set; }

    /// <summary>
    /// The brightness of this <see cref="HueLightGroup"/>
    /// </summary>
    public double? Brightness { get; set; }

    /// <summary>
    /// Applies the attributes of this class to a HueResource of type HueLightGroup.
    /// </summary>
    /// <param name="resource">The HueLightGroup to apply the attributes of this class to.</param>
    public override void Apply(HueResource resource)
    {
        if (resource is HueLightGroup group)
        {
            if (On.HasValue)
            {
                group.On = On;
            }
            if (Brightness != null)
            {
                group.Brightness = Brightness; 
            }
        }
    }
}