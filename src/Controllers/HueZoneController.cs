using System.Text.Json;
using JsonConversion;

namespace NetHue;

/// <summary>
/// Class for containing Hue zones configured on a HueBridge.
/// </summary>
public class HueZoneController : HueController
{
    /// <inheritdoc/>
    public HueZoneController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueZoneController(HueConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueZoneController(HueRepository respository) : base(respository) { }

    /// <summary>
    /// Fetches a list of zones configured on a Hue bridge.
    /// </summary>
    /// <returns>List of HueZones</returns>
    public async Task<List<HueZone>> GetZones()
    {
        var response = await Repository.Get("/resource/zone");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var zones = new List<HueZone>();
        foreach (JsonElement zoneData in rootElement.GetProperty("data").EnumerateArray())
        {
            var zone = SimpleJson.Convert<HueZone>(zoneData);
            if (zone != null)
            {
                zones.Add(zone!);
            }
        }
        return zones;
    }
}