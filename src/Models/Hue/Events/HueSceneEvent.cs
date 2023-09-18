using JsonConversion;

namespace NetHue;

/// <summary>
/// Class containing information on an event which occures 
/// </summary>
[SimpleJsonConverter(typeof(HueSceneEventJsonConverter))]
public class HueSceneEvent : HueResourceEvent
{

    /// <summary>
    /// The status of a scene.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// TODO: The actions of a scene.  
    /// </summary>
    public string? Actions { get; set; }

    /// <summary>
    /// TODO: The palette of a scene. 
    /// </summary>
    public string? Palette { get; set; }

    /// <summary>
    /// The dynamic palette of a scene. 
    /// </summary>
    public double? Speed { get; set; }

    /// <summary>
    /// The AutoDynamic value of a scene.
    /// </summary>
    public bool? AutoDynamic { get; set; }

    /// <summary>
    /// Applies the attributes of this class to a HueResource of type HueScene.
    /// </summary>
    /// <param name="resource">The HueScene to apply these attributes to.</param>
    public override void Apply(HueResource resource)
    {
        if (resource is HueScene scene)
        {
            if (Status != null)
            {
                scene.Status = Status;
            }

            if (Speed.HasValue)
            {
                scene.Speed = Speed.Value;
            }

            if (AutoDynamic.HasValue)
            {
                scene.AutoDynamic = AutoDynamic.Value;
            }
        }
    }
}