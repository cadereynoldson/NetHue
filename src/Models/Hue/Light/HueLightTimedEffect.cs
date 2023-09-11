namespace NetHue;

/// <summary>
/// Record containing information on a timed effect of a Hue Light. 
/// </summary>
public record HueLightTimedEffect : HueLightEffect
{
    public int Duration;
}