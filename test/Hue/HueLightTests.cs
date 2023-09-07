namespace HueTests;

public class HueLightTests
{

    private readonly string LIGHT_NAME_CONTAINS = "Lamp";

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

    /// <summary>
    /// Test for turning on all lights where their name contains "lamp" 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TurnOffAndOnHueLights()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        /// Turn off HueLights. 
        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(light, new HueLightStateBuilder().Off());
        }

        /// Turn on HueLights. 
        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(light, new HueLightStateBuilder().On());
        }
    }

    /// <summary>
    /// Test for turning all lights where their name contains the value in LIGHT_NAME_CONTAINS to a random color. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RandomHueLightColor()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        // Change to random colors: 
        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(RgbColor.Random(), light.CieColorGamut)
            );
        }
    }

    /// <summary>
    /// Test for turning all lights where their name contains the value in LIGHT_NAME_CONTAINS to a random color. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GreenHueLightColor()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        /// Change to random colors: 
        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(RgbColor.Green, light.CieColorGamut)
            );
        }
    }

    /// <summary>
    /// Test for turning all lights where their name contains the value in LIGHT_NAME_CONTAINS to a random color. 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestHueLightColor()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        var color = RgbColor.Teal;

        /// Change all lights to a color: 
        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(color, light.CieColorGamut)
            );
        }
    }

    [Fact]
    public async Task MinBrightness()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Brightness(0.1)
            );
        }
    }

    [Fact]
    public async Task MaxBrightness()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Brightness(100)
            );
        }
    }

    [Fact]
    public async Task MinMired()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(new MiredColor { MiredValue = 153 })
            );
        }
    }

    [Fact]
    public async Task MaxMired()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(new MiredColor { MiredValue = 500 })
            );
        }
    }

    [Fact]
    public async Task GetLightsWithPowerupMode()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        hueLights = hueLights.Where(l => l.State.Powerup != null).ToList();
        hueLights = hueLights.Where(l => l.State.Powerup!.Preset != HueLightPowerup.PowerupPreset.SAFETY).ToList();

        foreach (HueLight light in hueLights)
        {
            await Controller.UpdateLightState(
                light,
                new HueLightStateBuilder().Color(new MiredColor { MiredValue = 500 })
            );
        }
    }

    [Fact]
    public async Task PartyMode()
    {
        List<HueLight> hueLights = await Controller.GetLights();
        // Filter on lights that have the name "Lamp" in it. 
        hueLights = hueLights.Where(l => l.Name.Contains(LIGHT_NAME_CONTAINS)).ToList();

        for (int i = 0; i < 10; i++)
        {
            /// Change to random colors: 
            foreach (HueLight light in hueLights)
            {
                await Controller.UpdateLightState(
                    light,
                    new HueLightStateBuilder().Color(RgbColor.Random(), light.CieColorGamut)
                );
            }
            Thread.Sleep(1000);
        }
    }
}