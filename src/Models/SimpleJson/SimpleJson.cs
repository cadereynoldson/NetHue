namespace JsonConversion;

using System.Text.Json;

/// <summary>
/// Class designed to convert JSON Element objects and JSON strings to its corresponding object.
/// </summary>
public class SimpleJson
{
    /// <summary>
    /// Converts json element to an object of type T.  
    /// <param name="json">The JSON element to convert.</param>
    /// </summary> 
    public static T? Convert<T>(JsonElement json)
    {
        var attribute = typeof(T).GetCustomAttributes(typeof(SimpleJsonConverterAttribute), true)
                                     .OfType<SimpleJsonConverterAttribute>()
                                     .FirstOrDefault();

        if (attribute != null)
        {
            return Activator.CreateInstance(attribute.ConverterType) is ISimpleJsonConverter converter ? (T)converter.Convert(json) : default;
        }

        throw new InvalidOperationException($"No DynamicToClassConverterAttribute found for type {typeof(T).Name}");
    }
}