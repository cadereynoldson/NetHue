using System.Text.Json;

namespace NetHue;

class HueZoneSimpleJsonConverter : HueSimpleJsonConverter
{
    public override object Convert(JsonElement data)
    {
        return new HueRoom
        {
            Id = ParseId(data),
            Name = ParseName(data),
            Archetype = ParseArchetype(data),
            Children = ParseResourceIdentifierList(data.GetProperty("children")),
            Services = ParseResourceIdentifierList(data.GetProperty("services"))
        };
    }
}