namespace HueTests;

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

    /// <summary>
    /// Confirms your HueBridge can successfully return a single light. 
    /// </summary>
    [Fact]
    public async Task GetHueLight()
    {
        // Get a list of lights
        List<HueLight> lights = await Controller.GetLights();
        Assert.NotEmpty(lights);

        // Attempt to get the first light by its id:
        HueLight light = await Controller.GetLight(lights[0].Id);
        Assert.NotNull(light);
        Assert.Equal(lights[0].Id, light.Id);
    }

    /// <summary>
    /// Tests that getting a HueLight by ID that doesn't exist fails. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetHueLightFails()
    {
        var nonExistantId = "00000000-0000-0000-0000-000000000000";
        try
        {
            HueLight light = await Controller.GetLight(nonExistantId);
        }
        catch (HueHttpException e)
        {
            Assert.Equal("HueRepository.GET() failed with status code: NotFound: <1>: Not Found", e.Message);
        }
    }
}