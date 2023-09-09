using System.Runtime.CompilerServices;
using System.Text.Json;
using JsonConversion;
using Newtonsoft.Json;

namespace NetHue;

/// <summary>
/// Class containing methods for interacting with a Hue scenes configured on a Hue bridge.
/// </summary>
public class HueSceneController : HueController
{
    /// <inheritdoc/>
    public HueSceneController(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueSceneController(HueConfiguration config) : base(config) { }

    /// <inheritdoc/>
    public HueSceneController(HueRepository respository) : base(respository) { }

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
    /// Gets a list of HueScenes for the given HueLocation
    /// </summary>
    /// <param name="location">The location to get the scenes of.</param>
    /// <returns>A list of HueScenes associated with a room.</returns>
    public async Task<List<HueScene>> GetScenes(HueLocation location)
    {
        return await GetScenes(location.Id);
    }

    /// <summary>
    /// Gets the scenes associated with a given group (Room/Zone)
    /// </summary>
    /// <param name="id">The ID of the location to fetch the scenes of.</param>
    /// <returns>A list of HueScenes</returns>
    public async Task<List<HueScene>> GetScenes(string id)
    {
        List<HueScene> scenes = await GetScenes();
        scenes = scenes.Where(s => s.Group.Id == id).ToList();
        return scenes;
    }

    /// <summary>
    /// Returns all active HueScenes of the Hue bridge.
    /// </summary>
    /// <returns>A list of all active HueScenes.</returns>
    public async Task<List<HueScene>> GetActiveScenes()
    {
        List<HueScene> scenes = await GetScenes();
        scenes = scenes.Where(s => s.Status != "inactive").ToList();
        return scenes;
    }

    /// <summary>
    /// Gets the active scene of a HueLocation.
    /// </summary>
    /// <param name="location">The location to get the scene of.</param>
    /// <returns>The active HueScene, null if no scene is active.</returns>
    public async Task<HueScene?> GetActiveScene(HueLocation location)
    {
        List<HueScene> scenes = await GetScenes(location.Id);
        var activeScene = scenes.Where(s => s.Status != "inactive").First();
        return activeScene;
    }

    /// <summary>
    /// Turns "on" the parameterized HueScene. Updates the parameterized HueScene in place.
    /// </summary>
    /// <param name="scene">The scene to set.</param>
    /// <param name="duration">Transition to the scene within the given timeframe.</param>
    /// <param name="scene">Overrides the scene's default brightness.</param>
    /// <returns>The passed in HueScene</returns>
    public async Task<HueScene> SetScene(HueScene scene, double? brightness = null, int? duration = null)
    {
        var recallDict = new Dictionary<string, object>
        {
            {"action", "active"}
        };

        if (brightness != null)
        {
            recallDict["dimming"] = new Dictionary<string, object>
            {
                { "brightness", brightness }
            };
        }

        if (duration != null)
        {
            recallDict["duration"] = duration;
        }

        var bodyDict = new Dictionary<string, object>
        {
            {"recall", recallDict}
        };


        string body = JsonConvert.SerializeObject(bodyDict);

        // Returned value is just the scenes id. HueRepository will throw any errors if there were some. 
        await Repository.Put($"resource/scene/{scene.Id}", body);

        // Get updated information on scene. Put requests don't return what type of scene was applied. 
        var updatedScene = await GetScene(scene.Id);
        scene.Status = updatedScene.Status;
        return scene;
    }

    /// <summary>
    /// Updates the brightness of a scene, does not write any values, overrides scene's default brightness. 
    /// </summary>
    /// <param name="scene">The scene to update the brightness of.</param>
    /// <param name="brightness"></param>
    public async void SetSceneBrightness(HueScene scene, double brightness)
    {
        var bodyDict = new Dictionary<string, object>
        {
            ["recall"] = new Dictionary<string, object>
            {
                ["dimming"] = new Dictionary<string, object>
                {
                    {"brightness", brightness}
                }
            }
        };

        string body = JsonConvert.SerializeObject(bodyDict);

        // Returned value is just the scenes id. HueRepository will throw any errors if there were some. 
        await Repository.Put($"resource/scene/{scene.Id}", body);
    }
}