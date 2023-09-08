using System.Runtime.CompilerServices;

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
            Thread.Sleep(1000);
        }
    }
}