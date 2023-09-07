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
            Actions = ParseSceneActionList(data.GetProperty("actions")),
            Group = ParseResourceIdentifier(data.GetProperty("group")),
            Palette = HasProperty(data, "palette") ? ParseHueScenePalette(data.GetProperty("palette")) : null
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
                    Gradient = HasProperty(data, "gradient") ? ParseHueGradient(data.GetProperty("gradient")) : null,
                    Effect = HasProperty(data, "effects") ? action.GetProperty("effects").GetProperty("effect").GetString() : null
                }
            );
        }
        return l;
    }

    /// <summary>
    /// Parses a HueScenePalette
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static HueScenePalette ParseHueScenePalette(JsonElement data)
    {
        return new HueScenePalette {
            Colors = ParseHuePaletteCieColorList(data.GetProperty("color")),
            Dimming = ParseDimmingList(data.GetProperty("dimming")),
            ColorTemperature = ParseColorTemperatureList(data.GetProperty())
        };
    }

    private static List<double> ParseDimmingList(JsonElement data)
    {
        var l = new List<double>();
        foreach (JsonElement d in data.EnumerateArray())
        {
            l.Add(d.GetDouble());
        }
        return l;
    }
}