using System.Text.Json;

namespace NetHue;

class HueRoomSimpleJsonConverter : HueSimpleJsonConverter
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

    public override string ToJson(object data)
    {
        throw new NotImplementedException();
    }
}