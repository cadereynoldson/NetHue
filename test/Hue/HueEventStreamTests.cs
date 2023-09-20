using System.Security.Cryptography.X509Certificates;
using NuGet.Frameworks;

public class HueEventStreamTests
{
    HueEventRepository Repository = new("Data/config.json");

    [Fact]
    public async Task TestEventStreamOnOff()
    {
        Thread.Sleep(1000);

        HueLightController lightController = new("Data/config.json");
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name!.Contains("Cade")).First();
        var lights = await lightController.GetLights(room);

        var resourceManager = new HueResourceManager().Manage(lights);
        Repository.StartEventStream(resourceManager);


        Thread.Sleep(1000);

        /// Turn off HueLights. 
        foreach (HueLight light in lights)
        {
            await lightController.UpdateLightState(light, new HueLightStateBuilder().Off());
        }

        Thread.Sleep(2000);
        foreach (HueLight light in lights)
        {
            Assert.False(light.On);
        }

        /// Turn on HueLights. 
        foreach (HueLight light in lights)
        {
            await lightController.UpdateLightState(light, new HueLightStateBuilder().On());
        }

        Thread.Sleep(1000);
        foreach (HueLight light in lights)
        {
            Assert.True(light.On);
        }
    }

    [Fact]
    public async Task TestEventStreamColorChange()
    {
        Thread.Sleep(1000);

        HueLightController lightController = new("Data/config.json");
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name!.Contains("Cade")).First();
        var lights = await lightController.GetLights(room);

        var resourceManager = new HueResourceManager().Manage(lights);
        Repository.StartEventStream(resourceManager);

        Thread.Sleep(1000);

        /// Turn off HueLights. 
        foreach (HueLight light in lights)
        {
            await lightController.UpdateLightState(light, new HueLightStateBuilder().Off());
        }

        Thread.Sleep(2000);
        foreach (HueLight light in lights)
        {
            Assert.False(light.On);
        }

        /// Turn on HueLights. 
        foreach (HueLight light in lights)
        {
            await lightController.UpdateLightState(light, new HueLightStateBuilder().On());
        }

        Thread.Sleep(1000);
        foreach (HueLight light in lights)
        {
            Assert.True(light.On);
        }
    }

    [Fact]
    public async Task TestEventStreamChangeScene()
    {
        var sceneController = new HueSceneController("Data/config.json");
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name!.Contains("Cade")).First();
        var scenes = await sceneController.GetScenes(room);

        // Start resource manager with the scenes we have.
        var resourceManager = new HueResourceManager().Manage(scenes);
        Repository.StartEventStream(resourceManager);

        // Set First Scene: 
        await sceneController.SetScene(scenes[0]);
        Thread.Sleep(500);
        // Set second scene
        await sceneController.SetScene(scenes[1]);
        Thread.Sleep(500);

        Assert.NotEqual("static", scenes[0].Status);
        Assert.Equal("static", scenes[1].Status);
    }
}