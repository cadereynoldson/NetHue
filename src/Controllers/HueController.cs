namespace NetHue; 

/// <summary>
/// Class which handles getting various types on information from a HueBridge.
/// Handles parsing, specific calls to endpoints, etc. 
/// </summary>
public abstract class HueController 
{

    /// <summary>
    /// The HueRepository to use to get data from the HueBridge.
    /// </summary>
    protected readonly HueRepository Repository; 

    /// <summary>
    /// Creates a new instance of a HueController. 
    /// </summary>
    /// <param name="configPath">The path to the HueBridgeConfiguration compatible file.</param>
    public HueController(string configPath) : this(HueBridgeConfiguration.FromFile(configPath)) { }

    /// <summary>
    /// Creates a new instance of a HueController.
    /// </summary>
    /// <param name="config">The HueBridgeConfiguration to use.</param>
    public HueController(HueBridgeConfiguration config) : this(new HueRepository(config)) { }

    /// <summary>
    /// Creates a new instance of a HueController.
    /// </summary>
    /// <param name="respository">The repository to use to access the HueBridge.</param>
    public HueController(HueRepository respository)
    {
        Repository = respository;
    }
}