namespace NetHue;

using System.Text.Json;

public class HueSceneSimpleJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueScene
        {
            Id = ParseId(data),
            Name = ParseName(data),
            Actions = ParseSceneActionList(data.GetProperty("actions"))
        };
    }

    public override string ToJson(object data)
    {
        throw new NotImplementedException();
    }

    private static List<HueSceneAction> ParseSceneActionList(JsonElement data)
    {
        var l = new List<HueSceneAction>();
        foreach (JsonElement action in data.EnumerateArray())
        {

            l.Add(
                new HueSceneAction
                {
                    Target = ParseResourceIdentifier(action.GetProperty("target")),
                    On = HasProperty(data, "on") ? action.GetProperty("on").GetProperty("on").GetBoolean() : null,
                    Color = HasProperty(data, "color") ? ParseCieColor(action.GetProperty("color").GetProperty("xy")) : null,
                    ColorTemperature = HasProperty(data, "color_temperature") ? ParseMiredColor(action.GetProperty("color_temperature").GetProperty("mirek")) : null,
                }
            );
        }
        return l;
    }
}