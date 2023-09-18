namespace NetHue;

/// <summary>
/// Record containing information on a timed effect of a Hue Light. 
/// </summary>
public record HueLightTimedEffect : HueLightEffect
{
    /// <summary>
    /// The duration of a HueLightTimedEffect.
    /// </summary>
    public int Duration;
}