using Newtonsoft.Json.Linq;

namespace Hue;

public class HueLightStateBuilder
{

    private readonly JObject State;

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
        AddOrUpdateProperty("on", new JObject(new JProperty("on", true)));
        return this;
    }

    /// <summary>
    /// Turns the HueLight this is applied to off. 
    /// </summary>
    /// <returns>This HueLightStateBuilder</returns>
    public HueLightStateBuilder Off()
    {
        AddOrUpdateProperty("on", new JObject(new JProperty("on", false)));
        return this;
    }

    /// <summary>
    /// Changes the color of a light to a specific CIE color. 
    /// </summary>
    /// <param name="color">The color to change the light to.</param>
    /// <returns></returns>
    public HueLightStateBuilder Color(CieColor color)
    {
        AddOrUpdateProperty(
            "color", 
            new JObject(
                new JProperty(
                    "xy", 
                    new JObject(
                        new JProperty("x", color.X),
                        new JProperty("y", color.Y)
                    )
                )
            )
        );
        return this;
    }

    /// <summary>
    /// Changes the color of a light to a RGB color. 
    /// </summary>
    /// <param name="color">The RGB color to change the light to.</param>
    /// <param name="cieColorGamut">The CIE color gamut to convert this color to.</param>
    /// <returns></returns>
    public HueLightStateBuilder Color(RgbColor color, CieColorGamut cieColorGamut)
    {
        return Color(color.ToCie(cieColorGamut));
    }

    /// <summary>
    /// Changes the color of a light to a MiredColor. 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public HueLightStateBuilder Color(MiredColor color)
    {
        AddOrUpdateProperty("color_temperature", new JObject(new JProperty("mirek", color.MiredValue)));
        return this; 
    }

    public HueLightStateBuilder Brightness(double brightness)
    {
        AddOrUpdateProperty("dimming", new JObject(new JProperty("brightness", brightness)));
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