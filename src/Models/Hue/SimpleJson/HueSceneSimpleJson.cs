namespace NetHue;

using System.Text.Json;

class HueSceneSimpleJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueScene
        {
            Id = ParseId(data),
            Name = ParseName(data),
            Actions = ParseSceneActionList(data.GetProperty("actions")),
            Group = ParseResourceIdentifier(data.GetProperty("group")),
            Palette = HasProperty(data, "palette") ? ParseHueScenePalette(data.GetProperty("palette")) : null,
            Speed = data.GetProperty("speed").GetDouble(),
            AutoDynamic = data.GetProperty("auto_dynamic").GetBoolean(),
            Status = data.GetProperty("status").GetProperty("active").GetString()!
        };
    }

    private static List<HueSceneAction> ParseSceneActionList(JsonElement data)
    {
        var l = new List<HueSceneAction>();
        foreach (JsonElement action in data.EnumerateArray())
        {
            var actionValues = action.GetProperty("action");
            l.Add(
                new HueSceneAction
                {
                    Target = ParseResourceIdentifier(action.GetProperty("target")),
                    On = HasProperty(actionValues, "on") ? actionValues.GetProperty("on").GetProperty("on").GetBoolean() : null,
                    Color = HasProperty(actionValues, "color") ? ParseCieColor(actionValues.GetProperty("color").GetProperty("xy")) : null,
                    ColorTemperature = HasProperty(actionValues, "color_temperature") ? ParseMiredColor(actionValues.GetProperty("color_temperature").GetProperty("mirek")) : null,
                    Gradient = HasProperty(actionValues, "gradient") ? ParseHueGradient(actionValues.GetProperty("gradient")) : null,
                    Effect = HasProperty(actionValues, "effects") ? actionValues.GetProperty("effects").GetProperty("effect").GetString() : null
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
        return new HueScenePalette
        {
            Colors = ParseHuePaletteCieColorList(data.GetProperty("color")),
            Dimming = ParseDimmingList(data.GetProperty("dimming")),
            ColorTemperature = ParseHueColorTemperatureList(data.GetProperty("color_temperature"))
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