using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using JsonConversion;

namespace NetHue;

public class HueSceneController : HueController
{
    /// <inheritdoc/>
    public HueSceneController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueSceneController(HueBridgeConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueSceneController(HueRepository respository) : base(respository) { }

    /// <summary>
    /// Gets a list of all the HueScenes associated with the hue bridge. 
    /// </summary>
    /// <returns>A list of all the HueScenes associated with the hue bridge. </returns>
    public async Task<List<HueScene>> GetScenes()
    {
        var response = await Repository.Get("resource/scene");

        // Fetch information from object. 
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        var scenes = new List<HueScene>();
        foreach (JsonElement roomData in rootElement.GetProperty("data").EnumerateArray())
        {
            var scene = SimpleJson.Convert<HueScene>(roomData);
            if (scene != null)
            {
                scenes.Add(scene!);
            }
        }
        return scenes;
    }

    /// <summary>
    /// Gets a list of HueScenes for the given HueRoom
    /// </summary>
    /// <param name="room">The room to get the scenes of.</param>
    /// <returns>A list of HueScenes associated with a room.</returns>
    public async Task<List<HueScene>> GetScenes(HueRoom room)
    {
        return await GetScenes(room.Id);
    }

    /// <summary>
    /// Gets the scenes associated with a given group (Room/Zone)
    /// </summary>
    /// <param name="groupId">The ID of the group to fetch the scenes of.</param>
    /// <returns>A list of HueScenes</returns>
    public async Task<List<HueScene>> GetScenes(string groupId)
    {
        List<HueScene> scenes = await GetScenes();
        scenes = scenes.Where(s => s.Group.Id == groupId).ToList();
        return scenes;
    }

    /// <summary>
    /// Turns "on" the parameterized HueScene. 
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public async Task<HueScene> SetScene(HueScene scene)
    {
        string body = "{\"recall\": {\"action\": \"active\"}}";
        string response = await Repository.Put($"resource/scene/{scene.Id}", body);
        return scene;
    }
}