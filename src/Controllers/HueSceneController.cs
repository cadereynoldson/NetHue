using System.Text.Json;
using JsonConversion;

namespace NetHue;

public class HueSceneController : HueController
{
    /// <inheritdoc/>
    public HueSceneController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueSceneController(HueConfiguration config) : base(config) { }

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
    /// Gets a HueScene given its ID. 
    /// </summary>
    /// <param name="id">The ID of the scene to fetch.</param>
    /// <returns>The HueScene with the given id</returns>
    public async Task<HueScene> GetScene(string id)
    {
        var response = await Repository.Get($"resource/scene/{id}");
        using JsonDocument document = JsonDocument.Parse(response);
        var rootElement = document.RootElement;

        // Response will only contain one Scene. 
        var sceneData = rootElement.GetProperty("data").EnumerateArray().First();
        return SimpleJson.Convert<HueScene>(sceneData)!;
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
    /// Turns "on" the parameterized HueScene. Updates the parameterized HueScene in place.
    /// </summary>
    /// <param name="scene">The scene to set.</param>
    /// <returns>The passed in HueScene</returns>
    public async Task<HueScene> SetScene(HueScene scene)
    {
        string body = "{\"recall\": {\"action\": \"active\"}}";
        
        // Returned value is just the scenes id. HueRepository will throw any errors if there were some. 
        await Repository.Put($"resource/scene/{scene.Id}", body);
        
        // Get updated information on scene. Put requests don't return what type of scene was applied. 
        var updatedScene = await GetScene(scene.Id);
        scene.Status = updatedScene.Status;
        return scene;
    }
}