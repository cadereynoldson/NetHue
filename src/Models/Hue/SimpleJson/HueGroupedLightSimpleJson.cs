using System.Text.Json;

namespace NetHue; 

/// <summary>
/// Class for parsing HueGroupedLights from json elements.
/// </summary>
public class HueGroupedLightSimpleJsonConverter : HueSimpleJsonConverter
{
    /// <summary>
    /// Creates a HueGroupedLight from a JsonElement directly containing the information of a HueGroupedLight. 
    /// </summary>
    /// <param name="data">The data directly containing a </param>
    /// <returns></returns>
    public override object Convert(JsonElement data)
    {
        return new HueGroupedLights
        {
            Id = data.GetProperty("id").GetString()!,
            Owner = ParseResourceIdentifier(data.GetProperty("owner")),
            On = HasProperty(data, "on") ? data.GetProperty("on").GetProperty("on").GetBoolean() : null,
            Brightness = HasProperty(data, "dimming") ? data.GetProperty("dimming").GetProperty("brightness").GetDouble() : null,
            AlertActionValues = HasProperty(data, "alert") ? ParseStringList(data.GetProperty("alert").GetProperty("action_values")) : null,
            SignalingValues = HasProperty(data, "signaling") ? ParseStringList(data.GetProperty("signaling").GetProperty("signal_values")) : null
        };
    }

    public override string ToJson(object data)
    {
        throw new NotImplementedException();
    }
}