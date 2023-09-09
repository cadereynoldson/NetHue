namespace NetHue;

using JsonConversion;

/// <summary>
/// Record containing data on a configured "room". Rooms group devices and each device can only be part of one room. 
/// </summary>
[SimpleJsonConverter(typeof(HueRoomSimpleJsonConverter))]
public record HueRoom : HueLocation { }