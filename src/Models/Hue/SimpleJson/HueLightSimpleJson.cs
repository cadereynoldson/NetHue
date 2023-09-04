namespace NetHue;

using System.Net.NetworkInformation;
using System.Text.Json;
using JsonConversion;

/// <summary>
/// Class which handles parsing Json 
/// </summary>
public class HueLightSimpleJsonConverter : ISimpleJsonConverter
{
    /// <summary>
    /// Creates a HueLight from dynamic JSON data provided from the Hue API. 
    /// </summary>
    /// <param name="data">The data to create a HueLight from. </param>
    /// <returns></returns>
    public object Convert(JsonElement data)
    {
        return new HueLight
        {
            Id = data.GetProperty("id").GetString()!,
            Name = data.GetProperty("metadata").GetProperty("name").GetString()!,
            State = new HueLightState 
            {
                On = data.GetProperty("on").GetProperty("on").GetBoolean(),
                Brightness = data.GetProperty("dimming").GetProperty("brightness").GetDouble(),
                Cie = ParseCieColor(data.GetProperty("color").GetProperty("xy")),
                ColorTemperature = new MiredColor
                {
                    // value under "mirek_valid" will indicate if there is a non-null value under "mirek"
                    MiredValue = data.GetProperty("color_temperature").GetProperty("mirek_valid").GetBoolean() ?
                        data.GetProperty("color_temperature").GetProperty("mirek").GetInt32() : 153
                },
                Dynamics = new HueLightDynamics
                {
                    Status = data.GetProperty("dynamics").GetProperty("status").GetString(),
                    Speed = data.GetProperty("dynamics").GetProperty("speed").GetDouble(),
                    SpeedValid = data.GetProperty("dynamics").GetProperty("speed_valid").GetBoolean(),
                    DynamicStatusValues = ParseStringList(data.GetProperty("dynamics").GetProperty("status_values"))
                },
                Signaling = ParseSignalingState(data.GetProperty("signaling")),
                Mode = data.GetProperty("mode").GetString()!,
                Gradient = ParseHueLightGradient(data),
                Effect = ParseHueLightEffect(data),
                TimedEffect = ParseHueLightTimedEffect(data)
            },
            MiredColorRange = new MiredColorRange
            {
                Minimum = data.GetProperty("color_temperature").GetProperty("mirek_schema").GetProperty("mirek_minimum").GetInt32(),
                Maximum = data.GetProperty("color_temperature").GetProperty("mirek_schema").GetProperty("mirek_maximum").GetInt32()
            },
            CieColorGamut = new CieColorGamut
            {
                Red = ParseCieColor(data.GetProperty("color").GetProperty("gamut").GetProperty("red")),
                Green = ParseCieColor(data.GetProperty("color").GetProperty("gamut").GetProperty("green")),
                Blue = ParseCieColor(data.GetProperty("color").GetProperty("gamut").GetProperty("blue"))
            },
            AlertActionValues = ParseStringList(data.GetProperty("alert").GetProperty("action_values")),
        };
    }

    public string ToJson(object data)
    {
        throw new NotImplementedException();
    }

    private static CieColor ParseCieColor(JsonElement data)
    {
        return new CieColor(data.GetProperty("x").GetDouble(), data.GetProperty("y").GetDouble());
    }

    private static HueLightEffect? ParseHueLightEffect(JsonElement data)
    {
        if (data.TryGetProperty("effect", out JsonElement effect))
        {
            return new HueLightEffect
            {
                Effect = effect.GetProperty("effect").GetString()!,
                StatusValues = ParseStringList(effect.GetProperty("status_values")),
                Status = effect.GetProperty("status").GetString()!,
                EffectValues = ParseStringList(effect.GetProperty("effect_values"))
            };
        }
        return null;
    }

    private static HueLightTimedEffect? ParseHueLightTimedEffect(JsonElement data)
    {
        if (data.TryGetProperty("effect", out JsonElement effect))
        {
            return new HueLightTimedEffect
            {
                Effect = effect.GetProperty("effect").GetString()!,
                StatusValues = ParseStringList(effect.GetProperty("status_values")),
                Status = effect.GetProperty("status").GetString()!,
                EffectValues = ParseStringList(effect.GetProperty("effect_values")),
                Duration = ParseIntOrDefault(effect.GetProperty("duration"))
            };
        }
        return null;
    }

    private static HueLightGradient? ParseHueLightGradient(JsonElement data)
    {
        if (data.TryGetProperty("gradient", out JsonElement gradient))
        {
            var colors = new List<CieColor>();
            foreach (var element in gradient.GetProperty("points").EnumerateArray())
            {
                colors.Add(ParseCieColor(element.GetProperty("color").GetProperty("xy")));
            }

            return new HueLightGradient
            {
                Points = colors,
                GradientMode = gradient.GetProperty("mode").GetString()!,
                GradientPointsCapable = ParseIntOrDefault(gradient.GetProperty("points_capable")),
                GradientModeValues = ParseStringList(gradient.GetProperty("mode_values")),
                GradientPixelCount = ParseIntOrDefault(gradient.GetProperty("pixel_count"))
            };
        }
        return null;
    }

    private static HueLightSignaling? ParseSignalingState(JsonElement data)
    {
        if (data.TryGetProperty("status", out JsonElement status))
        {
            string signal = status.GetProperty("signal").GetString()!;
            return new HueLightSignaling
            {
                Signal = signal,
                EstimatedEnd = signal.Equals("no_signal") ? null : status.GetProperty("estimated_end").GetDateTime(),
                Colors = ParseCieColorList(status.GetProperty("colors")),
                SignalValues = ParseStringList(data.GetProperty("signaling").GetProperty("signal_values")),
            };
        }
        return null;
    }

    private static List<string> ParseStringList(JsonElement data)
    {
        var l = new List<string>();
        foreach (JsonElement str in data.EnumerateArray())
        {
            l.Add(str.GetString()!);
        }
        return l;
    }

    private static List<CieColor> ParseCieColorList(JsonElement data)
    {
        var l = new List<CieColor>();
        foreach (JsonElement color in data.EnumerateArray())
        {
            l.Add(ParseCieColor(color.GetProperty("xy")));
        }
        return l;
    }

    private static int ParseIntOrDefault(JsonElement data, int onFail = 0)
    {
        try {
            return data.GetInt32();
        } catch (Exception) {
            return 0;
        }
    }
}