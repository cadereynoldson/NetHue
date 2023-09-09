using System.Text.Json;
using JsonConversion;

namespace NetHue;

/// <summary>
/// Class containing methods for interacting with rooms configured on a Hue bridge.
/// </summary>
public class HueRoomController : HueController
{
    /// <inheritdoc/>
    public HueRoomController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueRoomController(HueConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueRoomController(HueRepository respository) : base(respository) { }

    /// <summary>
    /// Gets all of the rooms configured with the connected hue bridge. 
    /// </summary>
    /// <returns>A list of HueRooms./returns>
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