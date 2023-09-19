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
    /// <exception cref="HueHttpException">On non-successful fetching of lights.</exception>
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
    /// <returns> If found, a <see cref="HueLightGroup"/> with the given ID.</returns>
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
    /// <exception cref="HueHttpException">On non-successful fetching of the light groups.</exception>
    /// <returns>A list of all the grouped lights connected to the HueBridge.</returns>
    public async Task<List<HueLightGroup>> GetLightGroups()
    {
        string response = await Repository.Get("resource/grouped_light");

        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var groupedLights = new List<HueLightGroup>();
        foreach (JsonElement element in rootElement.GetProperty("data").EnumerateArray())
        {
            groupedLights.Add(SimpleJson.Convert<HueLightGroup>(element)!);
        }

        return groupedLights;
    }

    /// <summary>
    /// Gets the HueLightGroups of the specified location. 
    /// </summary>
    /// <param name="location">The the location to get the lights of.</param>
    /// <exception cref="HueHttpException">On non-successful fetching of the light group.</exception>
    /// <returns>The HueLightGroup of a room</returns>
    public async Task<HueLightGroup?> GetLightGroup(HueLocation location)
    {
        return await GetLightGroup(location.Id);
    }

    /// <summary>
    /// Gets the HueLightGroups of the specified location. 
    /// </summary>
    /// <param name="locationId">The id of the location to get the lights of.</param>
    /// <exception cref="HueHttpException">On non-successful fetching of the light group.</exception>
    /// <returns>The HueLightGroup of a room</returns>
    public async Task<HueLightGroup?> GetLightGroup(string locationId)
    {
        var lights = await GetLightGroups();
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
    /// Updates a <see cref="HueLight"/> with a new state.
    /// For updates of passed in <see cref="HueLightGroup"/>, use HueResourceManager/HueEventRespository.
    /// </summary>
    /// <param name="light">The light to update.</param>
    /// <param name="state"></param>
    /// <exception cref="HueHttpException">On non-successful updating of the light state.</exception>
    public async Task UpdateLightState(HueLight light, HueLightStateBuilder state)
    {
        string body = state.ToString();
        await Repository.Put($"resource/light/{light.Id}", body);
    }

    /// <summary>
    /// Updates the state of grouped hue lights. 
    /// For updates of passed in HueLightGroup, use HueResourceManager/HueEventRespository.
    /// </summary>
    /// <param name="lights">The lights to update the state of.</param>
    /// <param name="state">The state to apply to the lights.</param>
    /// <exception cref="HueHttpException">On non-successful updating of the light group state.</exception>
    public async Task UpdateLightGroupState(HueLightGroup lights, HueLightStateBuilder state)
    {
        string body = state.ToString();
        await Repository.Put($"resource/grouped_light/{lights.Id}", body);
    }
}