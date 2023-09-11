using System.Runtime.CompilerServices;
using Xunit.Sdk;

namespace NetHue;

public class HueSceneTests
{
    private readonly HueSceneController Controller = new("Data/config.json");

    [Fact]
    public async Task GetScenes()
    {
        List<HueScene> scenes = await Controller.GetScenes();
        Assert.NotEmpty(scenes);
    }

    [Fact]
    public async Task GetScenesByRoom()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        Assert.NotEmpty(scenes);
    }

    /// <summary>
    /// Test rotating the scenes of a given room is successful. 
    /// </summary>
    [Fact]
    public async Task RotateScenes()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        foreach (var scene in scenes)
        {
            await Controller.SetScene(scene);
            Assert.Equal("static", scene.Status);
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// Test rotating the scenes of a given room is successful. 
    /// </summary>
    [Fact]
    public async Task RotateScenesWithDimming()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        foreach (var scene in scenes)
        {
            await Controller.SetScene(scene, 50);
            Assert.Equal("static", scene.Status);
            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// Test rotating the scenes of a given room is successful. 
    /// </summary>
    [Fact]
    public async Task RotateScenesWithDimmingAndDuration()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        foreach (var scene in scenes)
        {
            await Controller.SetScene(scene, 50, 1);
            Assert.Equal("static", scene.Status);
            Thread.Sleep(100);
        }
    }


    [Fact]
    public async Task GetActiveScenes()
    {
        var scenes = await Controller.GetActiveScenes();
        Assert.NotEmpty(scenes);
    }

    [Fact]
    public async Task GetActiveRoomScenes()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        var scene = await Controller.GetActiveScene(room);
        Assert.NotNull(scene);
    }

    [Fact]
    public async Task SetSceneBrightness()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Cade")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        var scene = scenes.Last();
        await Controller.SetScene(scene);

        for (int i = 0; i <= 100; i += 10)
        {
            await Controller.SetScene(scene, i);
            Thread.Sleep(100);
        }
    }

    [Fact]
    public async Task Strobe()
    {
        var roomController = new HueRoomController("Data/config.json");
        var rooms = await roomController.GetRooms();
        var room = rooms.Where(r => r.Name.Contains("Living")).First();

        List<HueScene> scenes = await Controller.GetScenes(room);
        for (int i = 0; i < 5; i++)
        {
            foreach (var scene in scenes)
            {
                await Controller.SetScene(scene, brightness: 40, duration: 1);
                Assert.Equal("static", scene.Status);
                Thread.Sleep(100);
            }
        }
    }
}