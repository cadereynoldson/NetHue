using Newtonsoft.Json;

namespace Hue;

/// <summary>
/// Class containing the information for accessing a HueBridge via the Clip API V2.
/// Contains fields Ip, and AppKey. See <see href= "https://developers.meethue.com/develop/hue-api-v2/getting-started/"> Getting started with the Hue API V2 </see>
/// </summary>
public class HueBridgeConfiguration
{
    /// <summary>
    /// The IP of the HueBridge to connect to. 
    /// </summary>
    public readonly string Ip;

    /// <summary>
    /// The key which grants us access to the Hue Clip API v2.
    /// See <see href= "https://developers.meethue.com/develop/hue-api-v2/getting-started/"> Getting started with the Hue API V2 </see>
    /// </summary>
    public readonly string AppKey;

    /// <summary>
    /// Creates a new instance of a HueBridgeConfiguration. 
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="appKey">The key which grants us access to the Hue Clip API v2. </param>
    public HueBridgeConfiguration(string ip, string appKey)
    {
        Ip = ip;
        AppKey = appKey;
    }

    /// Parses a HueBridgeConfiguration from a configuration file.
    /// This file should be in the following format: 
    /// { 
    ///   "ip": "IP_VALUE",
    ///   "appKey": "APP_KEY_VALUE"
    /// }
    /// </summary>
    /// <param name="configPath">The path to the configuration file.</param>
    /// <returns>A HueBridgeConfiguration created from the configuration file.</returns>
    /// <exception cref="HueBridgeConfigurationException">On invalid configuration file input.</exception>
    public static HueBridgeConfiguration FromFile(string configPath)
    {
        string content = File.ReadAllText(configPath);
        try 
        {
            return JsonConvert.DeserializeObject<HueBridgeConfiguration>(content)!;
        } catch (Exception e)
        {
            throw new HueBridgeConfigurationException(e.Message);
        }
    }
}