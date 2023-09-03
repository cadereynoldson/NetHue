using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace Hue; 

public class HueLightStateBuilder
{

    private JObject State;

    public HueLightStateBuilder()
    {
        State = new JObject();
    }

    /// <summary>
    /// Turns the HueLight this is applied to on.
    /// </summary>
    /// <returns></returns>
    public HueLightStateBuilder On()
    {
        AddOrUpdateProperty("on", true);
        return this;
    }

    /// <summary>
    /// Turns the HueLight this is applied to off. 
    /// </summary>
    /// <returns>This HueLightStateBuilder</returns>
    public HueLightStateBuilder Off()
    {
        AddOrUpdateProperty("on", false);
        return this;
    }

    /// <summary>
    /// Changes the color to CIE. 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public HueLightStateBuilder Cie(CieColor color)
    {
        var cie = new JObject()
        {
            { "x", color.X },
            { "y", color.Y }
        };

        AddOrUpdateProperty("color", new JProperty("xy", cie));
        return this; 
    } 

    public HueLightStateBuilder Brightness(float brightness)
    {
        AddOrUpdateProperty("dimming", new JProperty("brightness", brightness));
        return this; 
    }

    private void AddOrUpdateProperty(string property, object value)
    {
        if (State[property] != null)
        {
            State.Remove(property);
        }
        State.Add(new JProperty(property, value));
    }

    /// <summary>
    /// Converts the properties applied to this object to a JSON formatted string. 
    /// </summary>
    /// <returns>A JSON formatted string</returns>
    public override string ToString()
    {
        return State.ToString();
    }
}