using System.Runtime.InteropServices;

namespace HueTests;

public class HueLightGroupsTests
{

    private readonly HueLightController Controller = new("Data/config.json");

    private readonly HueRoomController RoomController = new("Data/config.json");

    [Fact]
    public async Task GetGroupedLights()
    {
        var lights = await Controller.GetLightGroups();
        Assert.NotEmpty(lights);
    }

    [Fact]
    public async Task GetRoomGroupedLights()
    {
        var room = (await RoomController.GetRooms()).Where(r => r.Name!.Contains("Cade")).First();
        var group = await Controller.GetLightGroup(room.Id);
        Assert.NotNull(group);
    }

    [Fact]
    public async Task SetGroupedLightsBrightness()
    {
        var room = (await RoomController.GetRooms()).Where(r => r.Name!.Contains("Cade")).First();
        var group = await Controller.GetLightGroup(room.Id);

        await Controller.UpdateLightGroupState(group!, new HueLightStateBuilder().Brightness(100));
    }

    [Fact]
    public async Task TurnLightsOff()
    {
        var room = (await RoomController.GetRooms()).Where(r => r.Name!.Contains("Cade")).First();
        HueLightGroup group = (await Controller.GetLightGroup(room.Id))!;

        await Controller.UpdateLightGroupState(group!, new HueLightStateBuilder().Off());
    }

    [Fact]
    public async Task TurnLightsOn()
    {
        var room = (await RoomController.GetRooms()).Where(r => r.Name!.Contains("Cade")).First();
        var group = await Controller.GetLightGroup(room.Id);

        await Controller.UpdateLightGroupState(group!, new HueLightStateBuilder().On());
    }
}