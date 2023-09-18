using System.Text.Json;
using JsonConversion;

namespace NetHue;

/// <summary>
/// Class containing methods 
/// </summary>
public class HueEventRepository : HueRepository
{

    /// <summary>
    /// Source for the token which will cancel the event stream.
    /// </summary>
    private CancellationTokenSource? EventStreamCancellationTokenSource;

    /// <inheritdoc/>
    public HueEventRepository(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueEventRepository(HueConfiguration config) : base(config) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resourceManager"></param>
    /// <param name="cancellationToken"></param>
    public async void StartEventStream(HueResourceManager resourceManager, CancellationToken? cancellationToken = null)
    {
        // If an event stream is already started, cancel it.
        EventStreamCancellationTokenSource?.Cancel();

        // Handle cancellation token logic
        if (cancellationToken.HasValue)
        {
            EventStreamCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value);
        }
        else
        {
            EventStreamCancellationTokenSource = new CancellationTokenSource();
        }
        var token = EventStreamCancellationTokenSource.Token;

        // Start event stream: 
        using HttpClient client = BuildHttpClient();
        var streamUrl = $"https://{Configuration.Ip}/eventstream/clip/v2";
        while (!token.IsCancellationRequested)
        {
            try
            {
                // Current stream from hue bridge. 
                using var streamReader = new StreamReader(await client.GetStreamAsync(streamUrl, token));
                while (!streamReader.EndOfStream)
                {
                    string? response = await streamReader.ReadLineAsync();
                    if (response != null)
                    {
                        // We have a response, lets parse data.
                        using JsonDocument document = JsonDocument.Parse(response);
                        var rootElement = document.RootElement;
                        var events = new List<HueResourceEvent>();

                        // For each response on the event list, will be an array.
                        foreach (var elements in rootElement.EnumerateArray())
                        {
                            foreach (var element in elements.GetProperty("data").EnumerateArray())
                            {
                                string resourceType = element.GetProperty("type").GetString()!;
                                switch (resourceType)
                                {
                                    case "light":
                                        events.Add(SimpleJson.Convert<HueLightEvent>(element)!);
                                        break;
                                }
                            }
                        }
                        resourceManager.Apply(events);
                    }
                }
            }
            catch { }
        }
    }
}