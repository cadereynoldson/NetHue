namespace HueTests;

public class HueZoneTests
{

    private readonly HueZoneController Controller = new("Data/config.json");

    [Fact]
    public async Task GetZones()
    {
        List<HueZone> zones = await Controller.GetZones();
        Assert.NotEmpty(zones);
    }
}