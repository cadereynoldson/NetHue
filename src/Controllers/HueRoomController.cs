using System.Text.Json;
using JsonConversion;

namespace NetHue;

public class HueRoomController : HueController
{
    /// <inheritdoc/>
    public HueRoomController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueRoomController(HueBridgeConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueRoomController(HueRepository respository) : base(respository) { }

    public async Task<List<HueRoom>> GetRooms()
    {
        var response = await Repository.Get("/resource/room");
        
        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var rooms = new List<HueRoom>();
        foreach (JsonElement roomData in rootElement.GetProperty("data").EnumerateArray())
        {
            var room = SimpleJson.Convert<HueRoom>(roomData);
            if (room != null)
            {
                rooms.Add(room!);
            }
        }
        return rooms;
    }
}