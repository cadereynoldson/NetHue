using System.Data;
using System.Text.Json;

namespace NetHue;

class HueLightEventJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueLightEvent
        {
            ResourceId = data.GetProperty("id").GetString()!,
            On = HasProperty(data, "on") ? data.GetProperty("on").GetProperty("on").GetBoolean() : null,
            Brightness = HasProperty(data, "dimming") ? data.GetProperty("dimming").GetProperty("brightness").GetDouble() : null,
            ColorTemperature = HasProperty(data, "color_temperature") ? ParseMiredColor(data.GetProperty("color_temperature").GetProperty("mirek")) : null,
            Color = HasProperty(data, "color") ? ParseCieColor(data.GetProperty("color").GetProperty("xy")) : null
        };
    }

}