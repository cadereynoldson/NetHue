namespace Hue;

using System.Text.Json;
using JsonConversion;


/// <summary>
/// Handles fetching data from a Phillips Hue Bridge. 
/// </summary>
public class HueRepository
{

    /// <summary>
    /// HueHttpHelper for making requests to the HueBridge. 
    /// </summary>
    private readonly HueHttpHelper http;

    /// <summary>
    /// Creates a new instance of HueRepository
    /// </summary>
    /// <param name="ip">The IP address of the HueBridge to fetch information from.</param>
    /// <param name="appKey"></param>
    public HueRepository(string ip, string appKey)
    {
        http = new HueHttpHelper(ip, appKey);
    }

    /// <summary>
    /// Gets a list of all lights associated with a HueBridge. 
    /// </summary>
    /// <returns>A list of HueLights associated with a HueBridge.</returns>
    /// <exception cref="HueHttpException">On non-successful fetching of lights.</exception>
    public async Task<List<HueLight>> GetLights()
    {
        // Make call
        string body = await http.Get("resource/light");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(body);
        var rootElement = document.RootElement;

        // If errors exist in return body, fail agressively. 
        if (rootElement.GetProperty("errors").GetArrayLength() > 0)
        {
            throw new HueHttpException(message: HueHttpHelper.ParseErrors(rootElement), response: body);
        }
        // Otherwise, no errors exist, parse light data. 
        else
        {
            var lights = new List<HueLight>();
            foreach (JsonElement lightData in rootElement.GetProperty("data").EnumerateArray())
            {
                var light = FromJson.Convert<HueLight>(lightData);
                if (light != null)
                {
                    lights.Add(light!);
                }
            }
            return lights;
        }
    }
}