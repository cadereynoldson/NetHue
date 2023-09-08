namespace HueTests;

/// <summary>
/// Class containing tests assuring the functionality of the HueBridgeConfiguration class. 
/// </summary>
public class HueBridgeConfigTests
{

    /// <summary>
    /// Creates a HueBridgeConfiguration with the example_config.json file.
    /// </summary>
    /// <param name="path"></param>
    [Theory]
    [InlineData("Data/example_config.json")]
    public void CreateSucceeds(string path)
    {
        var config = HueConfiguration.FromJson(path);
        Assert.NotNull(config);
        Assert.Equal("0.0.0.0", config.Ip);
        Assert.Equal("appKey", config.AppKey);
    }
}