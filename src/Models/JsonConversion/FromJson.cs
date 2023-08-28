namespace JsonConversion;

using System.ComponentModel;
using System.Text.Json;

/// <summary>
/// Class designed to convert JSON Element objects and JSON strings to its corresponding object.
/// <typeparam name="T">The type of object to be handled by the converter.</typeparam>
/// </summary>
public class FromJson
{
    /// <summary>
    /// Converts json element to an object of type T.  
    /// <param name="json">The JSON element to convert.</param>
    /// </summary> 
    public static T? Convert<T>(JsonElement json)
    {
        var attribute = typeof(T).GetCustomAttributes(typeof(FromJsonConverterAttribute), true)
                                     .OfType<FromJsonConverterAttribute>()
                                     .FirstOrDefault();

        if (attribute != null)
        {
            return Activator.CreateInstance(attribute.ConverterType) is IFromJsonConverter converter ? (T)converter.Convert(json) : default;
        }

        throw new InvalidOperationException($"No DynamicToClassConverterAttribute found for type {typeof(T).Name}");
    }

    /// <summary>
    /// Converts json formatted string to an object of type T. 
    /// <param name="json">The JSON formatted string to convert.</param>
    /// </summary> 
    public static T? Convert<T>(string json)
    {
        using (JsonDocument document = JsonDocument.Parse(json))
        {
            return Convert<T>(document.RootElement);
        }
    }
}