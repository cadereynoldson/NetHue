namespace Hue;

/// <summary>
/// Class containing information of a group of one or more <see cref="HueLight"/>
/// </summary>
class HueLightGroup {

    /// <summary>
    /// The type of this group. Possible Values:
    /// <list type="bullet">
    /// <item>
    /// <description> "0": Default value representing all lights. </description>
    /// </item>
    /// <item>
    /// <description> "Luminaire" </description>
    /// </item>
    /// <item>
    /// <description> "Lightsource" </description>
    /// </item>
    /// <item>
    /// <description> "Lightgroup" </description>
    /// </item>
    /// <item>
    /// <description> "Room" </description>
    /// </item>
    /// <item>
    /// <description> "Entertainment" </description>
    /// </item>
    /// <item>
    /// <description> "Zone" </description>
    /// </item>
    /// </list>
    /// </summary>
    string GroupType { get; set; } = default!; 

    /// <summary>
    /// A list of lights contained in this group. 
    /// </summary>
    List<HueLight> Lights { get; set; } = default!;
}