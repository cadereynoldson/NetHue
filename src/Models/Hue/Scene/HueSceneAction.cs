namespace NetHue;

public record HueSceneAction
{
    public HueResourceIdentifier Target { get; set; } = default!;
    public bool? On { get; set; }

    public double? Brightness { get; set; }

    public CieColor? Color { get; set; }

    public MiredColor? ColorTemperature { get; set; }

    public HueGradient? Gradient { get; set; }

    public string? Effect { get; set; }

    public int? DynamicDuration { get; set; }
    
}