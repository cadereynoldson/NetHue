using System.Text.Json;
using JsonConversion;

namespace Hue; 

/// <summary>
/// Class which handles getting various types on information from a HueBridge.
/// Handles parsing, specific calls to endpoints, etc. 
/// </summary>
public class HueController 
{

    /// <summary>
    /// The HueRepository to use to get data from the HueBridge.
    /// </summary>
    private readonly HueRepository Repository; 

    /// <summary>
    /// Creates a new instance of a HueController. 
    /// </summary>
    /// <param name="configPath">The path to the HueBridgeConfiguration compatible file.</param>
    public HueController(string configPath) : this(HueBridgeConfiguration.FromFile(configPath)) {}

    /// <summary>
    /// Creates a new instance of a HueController.
    /// </summary>
    /// <param name="config">The HueBridgeConfiguration to use.</param>
    public HueController(HueBridgeConfiguration config) : this(new HueRepository(config)) {}

    /// <summary>
    /// Creates a new instance of a HueController.
    /// </summary>
    /// <param name="respository">The repository to use to access the HueBridge.</param>
    public HueController(HueRepository respository)
    {
        Repository = respository;
    }

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

        // If errors exist in return body, fail agressively. 
        if (rootElement.GetProperty("errors").GetArrayLength() > 0)
        {
            throw new HueHttpException(message: HueRepository.ParseErrors(rootElement), response: body);
        }
        // Otherwise, no errors exist, parse light data. 
        else
        {
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
    }
}