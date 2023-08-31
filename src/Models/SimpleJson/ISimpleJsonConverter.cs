using System.Text.Json;

namespace JsonConversion;

/// <summary>
/// Interface defining methods which should convert dynamic data to data of a given class. 
/// </summary>
public interface ISimpleJsonConverter
{

    /// <summary>
    /// Converts dynamic json data provided to this method to an object containing said data. 
    /// <param name="data"> The JsonElement to convert to an object. </param>
    /// </summary>
    public object Convert(JsonElement data);

    /// <summary>
    /// Converts the data stored in an object to a JSON formatted string. 
    /// </summary>
    /// <param name="data">The object to convert to a JSON formatted string.</param>
    /// <returns></returns>
    public string ToJson(object data);
}