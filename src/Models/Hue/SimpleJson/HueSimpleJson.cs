namespace NetHue;

using System.Text.Json;
using JsonConversion;

public abstract class HueSimpleJsonConverter : ISimpleJsonConverter
{
    /// <summary>
    /// Parses the ID of a HueResource given a JsonElement containing the property "id" 
    /// </summary>
    /// <param name="data">The data to parse the id from.</param>
    /// <returns>The ID of a HueResource.</returns>
    protected static string ParseId(JsonElement data)
    {
        return data.GetProperty("id").GetString()!;
    }

    /// <summary>
    /// Parses the name of a HueResource given a JsonElement containing the property "metadata" 
    /// </summary>
    /// <param name="data">The data to parse the name from.</param>
    /// <returns>The name of a HueResource.</returns>
    protected static string ParseName(JsonElement data)
    {
        return data.GetProperty("metadata").GetProperty("name").GetString()!;
    }

    /// <summary>
    /// Parses the archetype of a HueResource given a JsonElement containing the property "metadata" 
    /// </summary>
    /// <param name="data">The data to parse the archetype from.</param>
    /// <returns>The archetype of a HueResource.</returns>
    protected static string ParseArchetype(JsonElement data)
    {
        return data.GetProperty("metadata").GetProperty("archetype").GetString()!;
    }

    /// <summary>
    /// Parses the a CIE color from a JsonElement containing the properties "x" and "y" 
    /// </summary>
    /// <param name="data">The data to parse the name from.</param>
    /// <returns>The name of a HueResource.</returns>
    protected static CieColor ParseCieColor(JsonElement data)
    {
        return new CieColor { X = data.GetProperty("x").GetDouble(), Y = data.GetProperty("y").GetDouble() };
    }

    /// <summary>
    /// Parses the a mired color from a JsonElement directly containing an integer.  
    /// </summary>
    /// <param name="data">The JsonElement directly containing an integer.</param>
    /// <param name="onFail">The value to return on unable to parse a integer.</param>
    /// <returns>MiredColor with the value in data, or value on json fail.</returns>
    protected static MiredColor ParseMiredColor(JsonElement data, int onFail = 153)
    {
        return new MiredColor
        {
            MiredValue = ParseIntOrDefault(data, onFail)
        };
    }

    /// <summary>
    /// Parses a list of strings from a JsonElement directly containing an array. 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected static List<string> ParseStringList(JsonElement data)
    {
        var l = new List<string>();
        foreach (JsonElement str in data.EnumerateArray())
        {
            l.Add(str.GetString()!);
        }
        return l;
    }

    /// <summary>
    /// Parses a list of CieColors from a JsonElement directly containing and array of CieColors. 
    /// </summary>
    /// <param name="data">The JsonElement directly containing an array of CieColors</param>
    /// <returns>List of CieColors.</returns>
    protected static List<CieColor> ParseCieColorList(JsonElement data)
    {
        var l = new List<CieColor>();
        foreach (JsonElement color in data.EnumerateArray())
        {
            l.Add(ParseCieColor(color.GetProperty("xy")));
        }
        return l;
    }

    /// <summary>
    /// Parses a list of HueResourceIdentifiers from a JsonElement directly containing an array of Resource Identifiers. 
    /// </summary>
    /// <param name="data">The JsonElement directly containing an array of Resource identifiers.</param>
    /// <returns>List of HueResourceIdentifier</returns>
    protected static List<HueResourceIdentifier> ParseResourceIdentifierList(JsonElement data)
    {
        var l = new List<HueResourceIdentifier>();
        foreach (JsonElement resource in data.EnumerateArray())
        {
            l.Add(new HueResourceIdentifier
            {
                Id = resource.GetProperty("rid").GetString()!,
                ResourceType = resource.GetProperty("rtype").GetString()!
            });
        }
        return l;
    }

    /// <summary>
    /// Parses an integer from a JsonElement directly containing an integer. 
    /// </summary>
    /// <param name="data">The JsonElement containing an integer.</param>
    /// <param name="onFail">The value to return on failing to parse.</param>
    /// <returns>The integer stored in data, or the value provided in onFail.</returns>
    protected static int ParseIntOrDefault(JsonElement data, int onFail = 0)
    {
        try
        {
            return data.GetInt32();
        }
        catch (Exception)
        {
            return onFail;
        }
    }

    public abstract object Convert(JsonElement data);
    public abstract string ToJson(object data);
}