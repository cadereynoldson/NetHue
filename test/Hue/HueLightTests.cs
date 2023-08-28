using System.Runtime.InteropServices;

namespace Hue;


public class HueLightTests {
    
    private HueRepository Repository = new HueRepository("10.0.0.135", "fvan2ipditkame8HOU5SFGxwa3DczvWhXBYMIih0");
    
    /// <summary>
    /// Confirms that your HueBridge can successfully return a list of lights.
    /// </summary>
    [Fact]
    public async Task GetHueLights() {
        List<HueLight> lights = await Repository.GetLights();
        Assert.NotEmpty(lights);
    }
}