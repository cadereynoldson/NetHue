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

    public async void StartEventStream(CancellationToken? cancellationToken = null)
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
                    var response = await streamReader.ReadLineAsync();
                    Console.WriteLine($"Message: {response}");
                }
            }
            catch { }
        }
    }
}