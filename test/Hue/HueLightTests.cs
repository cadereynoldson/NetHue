namespace Hue;

public class HueLightTests
{

    private readonly HueLightController Controller = new("Data/config.json");

    /// <summary>
    /// Confirms that your HueBridge can successfully return a list of lights.
    /// </summary>
    [Fact]
    public async Task GetHueLights()
    {
        List<HueLight> lights = await Controller.GetLights();
        Assert.NotEmpty(lights);
    }
}