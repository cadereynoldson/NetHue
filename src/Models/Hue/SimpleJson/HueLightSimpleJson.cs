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
                ColorTemperature = ParseMiredColor(data.GetProperty("color_temperature").GetProperty("mirek")),
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
                TimedEffect = ParseHueLightTimedEffect(data),
                Powerup = ParsePowerup(data) 
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

    private static MiredColor ParseMiredColor(JsonElement data, int onFail = 153)
    {
        int value = onFail;
        try {
            value = data.GetInt32()!;
        } catch (Exception) {}

        return new MiredColor
        {
            MiredValue = value
        };
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
        try
        {
            return data.GetInt32();
        }
        catch (Exception)
        {
            return 0;
        }
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

    private static HueLightPowerup? ParsePowerup(JsonElement data)
    {
        
        if (data.TryGetProperty("powerup", out JsonElement powerup))
        {
            // Dimming may or may not exist: 
            double dimming = 0;
            HueLightPowerup.DimmingMode? dimmingBehavior = null;
            CieColor? cieColor = null;
            MiredColor? miredColor = null;
            HueLightPowerup.ColorMode? colorMode = null;
            
            if (powerup.TryGetProperty("dimming", out JsonElement dimmingElement))
            {
                if (dimmingElement.TryGetProperty("dimming", out JsonElement dimmingVals))
                {
                    dimming = dimmingVals.GetProperty("brightness").GetDouble();
                }
                // Required element. 
                dimmingBehavior = ParseDimmingBehavior(dimmingElement.GetProperty("mode"));
            }

            if (powerup.TryGetProperty("color", out JsonElement color))
            {
                if (color.TryGetProperty("color", out JsonElement colorElement))
                {
                    cieColor = ParseCieColor(colorElement.GetProperty("xy"));
                }
                if (color.TryGetProperty("color_temperature", out JsonElement tempElement))
                {
                    miredColor = ParseMiredColor(tempElement.GetProperty("mirek"));
                }
                // Required element
                colorMode = ParseColorBehavior(color.GetProperty("mode"));
            }

            return new HueLightPowerup
            {
                Preset = ParsePowerupPreset(powerup.GetProperty("preset")),
                Configured = powerup.GetProperty("configured").GetBoolean(),
                On = powerup.GetProperty("on").GetProperty("on").GetProperty("on").GetBoolean(),
                OnBehavior = ParseOnBehavior(powerup.GetProperty("on").GetProperty("mode")),
                Dimming = dimming,
                DimmingBehavior = dimmingBehavior,
                CieColor = cieColor,
                ColorTemperature = miredColor,
                ColorBehavior = colorMode
            };
        }
        return null;
    }

    private static HueLightPowerup.PowerupPreset ParsePowerupPreset(JsonElement data)
    {
        string preset = data.GetString()!;
        if (preset.Equals("safety"))
        {
            return HueLightPowerup.PowerupPreset.SAFETY;
        }
        else if (preset.Equals("powerfail"))
        {
            return HueLightPowerup.PowerupPreset.POWERFAIL;
        }
        else if (preset.Equals("last_on_state"))
        {
            return HueLightPowerup.PowerupPreset.LAST_ON_STATE;
        }
        else
        {
            return HueLightPowerup.PowerupPreset.CUSTOM;
        }
    }

    /// <summary>
    /// Parses HueLightPowerup.OnMode enum from a json element containing a string.
    /// </summary>
    /// <param name="data">The data to convert to a HueLightPowerup.OnMode enum</param>
    /// <returns>HueLightPowerup.OnMode, returns HueLightPowerup.OnMode.PREVIOUS by default.</returns>
    private static HueLightPowerup.OnMode ParseOnBehavior(JsonElement data)
    {
        string behavior = data.GetString()!;
        if (behavior.Equals("on"))
        {
            return HueLightPowerup.OnMode.ON;
        }
        else if (behavior.Equals("toggle"))
        {
            return HueLightPowerup.OnMode.TOGGLE;
        }
        else
        {
            return HueLightPowerup.OnMode.PREVIOUS;
        }
    }

    /// <summary>
    /// Parses a HueLightPowerup.DimmingMode enum from a json element directly containing a string. 
    /// </summary>
    /// <param name="data">The data to convert to a HueLightPowerup.DimmingMode enum</param>
    /// <returns>HueLightPowerup.DimmingMode, returns HueLightPowerup.DimmingMode.PREVIOUS by default.</returns>
    private static HueLightPowerup.DimmingMode ParseDimmingBehavior(JsonElement data)
    {
        string behavior = data.GetString()!;
        if (behavior.Equals("dimming"))
        {
            return HueLightPowerup.DimmingMode.DIMMING;
        }
        else
        {
            return HueLightPowerup.DimmingMode.PREVIOUS;
        }
    }

    /// <summary>
    /// Parses a HueLightPowerup.ColorMode enum from a json element that directly contains a string. 
    /// </summary>
    /// <param name="data">The data to parse a HueLightPowerup.ColorMode enum from.</param>
    /// <returns>HueLightPowerup.ColorMode, by default returns HueLightPowerup.ColorMode.PREVIOUS</returns>
    private static HueLightPowerup.ColorMode ParseColorBehavior(JsonElement data)
    {
        string behavior = data.GetString()!;
        if (behavior.Equals("color_temperature"))
        {
            return HueLightPowerup.ColorMode.MIRED;
        }
        else if (behavior.Equals("color"))
        {
            return HueLightPowerup.ColorMode.CIE;
        }
        else
        {
            return HueLightPowerup.ColorMode.PREVIOUS;
        }
    }
}