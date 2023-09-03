namespace Hue;

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
                Cie = CieFromJsonElement(data.GetProperty("color").GetProperty("xy")),
                ColorTemperature = new MiredColor
                {
                    // value under "mirek_valid" will indicate if there is a non-null value under "mirek"
                    MiredValue = data.GetProperty("color_temperature").GetProperty("mirek_valid").GetBoolean() ?
                        data.GetProperty("color_temperature").GetProperty("mirek").GetInt32() : 153
                },
            },
            MiredColorRange = new MiredColorRange
            {
                Minimum = data.GetProperty("color_temperature").GetProperty("mirek_schema").GetProperty("mirek_minimum").GetInt32(),
                Maximum = data.GetProperty("color_temperature").GetProperty("mirek_schema").GetProperty("mirek_maximum").GetInt32()
            },
            CieColorGamut = new CieColorGamut
            {
                Red = CieFromJsonElement(data.GetProperty("color").GetProperty("gamut").GetProperty("red")),
                Green = CieFromJsonElement(data.GetProperty("color").GetProperty("gamut").GetProperty("green")),
                Blue = CieFromJsonElement(data.GetProperty("color").GetProperty("gamut").GetProperty("blue"))
            }
        };
    }

    public string ToJson(object data)
    {
        throw new NotImplementedException();
    }

    private CieColor CieFromJsonElement(JsonElement data)
    {
        return new CieColor
        {
            X = data.GetProperty("x").GetDouble(),
            Y = data.GetProperty("y").GetDouble()
        };
    }
}