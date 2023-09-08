namespace NetHue;

using System.Text.Json;
using JsonConversion;

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
    /// Gets a light associated with the HueBridge by its ID. 
    /// </summary>
    /// <param name="id">The ID of the light to fetch.</param>
    /// <exception cref="HueHttpException">On non-successful fetching of the light.</exception>
    public async Task<HueLight> GetLight(string id)
    {
        // Make call
        string response = await Repository.Get($"resource/light/{id}");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        // Response will only contain one light. 
        var lightData = rootElement.GetProperty("data").EnumerateArray().First();
        return SimpleJson.Convert<HueLight>(lightData)!;
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

        // Response will contain _blank_
        var lightData = rootElement.GetProperty("data").EnumerateArray().First();
        return light;
    }
}