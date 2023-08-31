namespace JsonConversion;

/// <summary>
/// Class defining an attribute for custom implementations of DynamicConverters. 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SimpleJsonConverterAttribute : Attribute
{
    /// <summary>
    /// The object type this Attrubute points conversion to.   
    /// </summary>
    public Type ConverterType { get; }

    /// <summary>
    /// Creates a new instance of a FromJsonConverterAttribute.
    /// <param name=converterType> The object type this attribute handles conversion for. </param>
    /// </summary>
    public SimpleJsonConverterAttribute(Type converterType)
    {
        ConverterType = converterType;
    }
}