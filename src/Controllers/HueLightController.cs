namespace NetHue;

using System.Text.Json;
using JsonConversion;

/// <summary>
/// Class containing methods for interating with hue lights connected to a hue bridge. 
/// </summary>
public class HueLightController : HueController
{

    /// <inheritdoc/>
    public HueLightController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueLightController(HueConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueLightController(HueRepository respository) : base(respository) { }

    /// <summary>
    /// Gets a list of all lights associated with a HueBridge. 
    /// </summary>
    /// <returns>A list of HueLights associated with a HueBridge.</returns>
    /// <exception cref="HueHttpException">On non-successful fetching of lights.</exception>
    public async Task<List<HueLight>> GetLights()
    {
        // Make call
        string response = await Repository.Get("resource/light");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        // Parse lights
        var lights = new List<HueLight>();
        foreach (JsonElement lightData in rootElement.GetProperty("data").EnumerateArray())
        {
            var light = SimpleJson.Convert<HueLight>(lightData);
            if (light != null)
            {
                lights.Add(light!);
            }
        }
        return lights;
    }

    /// <summary>
    /// Fetches the lights of the parameterized HueRoom. 
    /// </summary>
    /// <param name="room">The room to get the lights of.</param>
    /// <returns>A list of HueLights</returns>
    public async Task<List<HueLight>> GetLights(HueLocation room)
    {
        var lights = await GetLights();

        var roomChildren = new HashSet<string>();
        foreach (var child in room.Children)
        {
            roomChildren.Add(child.Id);
        }

        lights = lights.Where(l => roomChildren.Contains(l.Owner.Id)).ToList();

        return lights;
    }

    /// <summary>
    /// Gets a light associated with the HueBridge by its ID. 
    /// </summary>
    /// <param name="id">The ID of the light to fetch.</param>
    /// <exception cref="HueHttpException">On non-successful fetching of the light.</exception>
    public async Task<HueLight?> GetLight(string id)
    {
        // Make call
        string response = await Repository.Get($"resource/light/{id}");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        // Response will only contain one light. 
        try
        {
            var lightData = rootElement.GetProperty("data").EnumerateArray().First();
            return SimpleJson.Convert<HueLight>(lightData)!;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a list of all the grouped lights connected to the HueBridge.
    /// </summary>
    /// <returns>A list of all the grouped lights connected to the HueBridge.</returns>
    public async Task<List<HueGroupedLights>> GetGroupedLights()
    {
        string response = await Repository.Get("resource/grouped_light");

        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var groupedLights = new List<HueGroupedLights>();
        foreach (JsonElement element in rootElement.GetProperty("data").EnumerateArray())
        {
            groupedLights.Add(SimpleJson.Convert<HueGroupedLights>(element)!);
        }

        return groupedLights;
    }

    /// <summary>
    /// Gets the HueGroupedLights of the specified location. 
    /// </summary>
    /// <param name="locationId">The id of the location to get the lights of.</param>
    /// <returns>The HueGroupedLights of a room</returns>
    public async Task<HueGroupedLights?> GetGroupedLights(string locationId)
    {
        var lights = await GetGroupedLights();
        try
        {
            return lights.Where(l => l.Owner.Id == locationId).First();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Updates a HueLight with a new state. Will mutate passed in HueLight object.
    /// </summary>
    /// <param name="light">The light to update.</param>
    /// <param name="state"></param>
    /// <returns>The updated/mutated passed in HueLight object.</returns>
    public async Task<HueLight> UpdateLightState(HueLight light, HueLightStateBuilder state)
    {
        string body = state.ToString();
        var response = await Repository.Put($"resource/light/{light.Id}", body);

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var lightData = rootElement.GetProperty("data").EnumerateArray().First();

        // TODO: MUTATE LIGHT
        return light;
    }

    /// <summary>
    /// Updates the state of grouped hue lights. 
    /// </summary>
    /// <param name="lights">The lights to update the state of.</param>
    /// <param name="state">The state to apply to the lights.</param>
    /// <returns>The updated HueGroupedLights</returns>
    public async Task<HueGroupedLights> UpdateGroupedLightsState(HueGroupedLights lights, HueLightStateBuilder state)
    {
        string body = state.ToString();
        var response = await Repository.Put($"resource/grouped_light/{lights.Id}", body);

        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var lightData = rootElement.GetProperty("data").EnumerateArray().First();

        // TODO: MUTATE LIGHT GROUP
        return lights;
    }
}