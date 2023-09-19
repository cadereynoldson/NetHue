using System.Text.Json;

namespace NetHue;

/// <summary>
/// Class for parsing a HueLightGroup from json elements.
/// </summary>
class HueLightGroupSimpleJsonConverter : HueSimpleJsonConverter
{
    /// <summary>
    /// Creates a HueLightGroup from a JsonElement directly containing the information of a HueLightGroup. 
    /// </summary>
    /// <param name="data">The data directly containing a </param>
    /// <returns></returns>
    public override object Convert(JsonElement data)
    {
        return new HueLightGroup
        {
            Id = data.GetProperty("id").GetString()!,
            Owner = ParseResourceIdentifier(data.GetProperty("owner")),
            On = HasProperty(data, "on") ? data.GetProperty("on").GetProperty("on").GetBoolean() : null,
            Brightness = HasProperty(data, "dimming") ? data.GetProperty("dimming").GetProperty("brightness").GetDouble() : null,
            AlertActionValues = HasProperty(data, "alert") ? ParseStringList(data.GetProperty("alert").GetProperty("action_values")) : null,
            SignalingValues = HasProperty(data, "signaling") ? ParseStringList(data.GetProperty("signaling").GetProperty("signal_values")) : null
        };
    }
}