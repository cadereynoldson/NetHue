using System.Text.Json;
using NetHue;

class HueLightGroupEventJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueLightGroupEvent
        {
            ResourceId = data.GetProperty("id").GetString()!,
            On = HasProperty(data, "on") ? data.GetProperty("on").GetProperty("on").GetBoolean() : null,
            Brightness = HasProperty(data, "dimming") ? data.GetProperty("dimming").GetProperty("brightness").GetDouble() : null,       
        };
    }
}