namespace NetHue;

using JsonConversion;

/// <summary>
/// Record containing data on a configured "zone". Zones group services and each service can be part of multiple zones.
/// </summary>
[SimpleJsonConverter(typeof(HueLocationSimpleJson))]
public record HueZone : HueLocation { }