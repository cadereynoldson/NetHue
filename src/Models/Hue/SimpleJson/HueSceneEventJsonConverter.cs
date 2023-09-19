using System.Text.Json;
using NetHue;

class HueSceneEventJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueSceneEvent
        {
            ResourceId = data.GetProperty("id").GetString()!,
            Status = HasProperty(data, "status") ? data.GetProperty("status").GetProperty("active").GetString()! : null,
            AutoDynamic = HasProperty(data, "auto_dynamic") ? data.GetProperty("auto_dynamic").GetBoolean() : null,
            Speed = HasProperty(data, "speed") ? data.GetProperty("speed").GetDouble() : null,
        };
    }
}