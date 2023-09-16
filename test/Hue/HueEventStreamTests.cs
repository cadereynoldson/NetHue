public class HueEventStreamTests
{
    HueEventRepository Repository = new("Data/config.json");

    [Fact]
    public async Task TestEventStream()
    {
        Repository.StartEventStream();
    }
}