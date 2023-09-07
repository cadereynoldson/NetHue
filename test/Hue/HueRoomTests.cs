namespace HueTests;

public class HueRoomTests
{

    private readonly HueRoomController Controller = new("Data/config.json");

    [Fact]
    public async Task GetRooms()
    {
        List<HueRoom> rooms = await Controller.GetRooms();
        Assert.NotEmpty(rooms);
    }
}