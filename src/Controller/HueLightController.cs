namespace Hue;

using System.Text.Json;
using JsonConversion;

public class HueLightController : HueController
{

    /// <inheritdoc/>
    public HueLightController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueLightController(HueBridgeConfiguration config) : base(config) { }

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
        string body = await Repository.Get("resource/light");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(body);
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
        string body = await Repository.Get($"resource/light/{id}");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(body);
        var rootElement = document.RootElement;

        return new HueLight();
    }
}