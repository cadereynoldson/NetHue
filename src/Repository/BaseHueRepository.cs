namespace NetHue;

/// <summary>
/// Base hue repository, stores information shared throughout HueRepositories
/// </summary>
public abstract class BaseHueRepository
{
    /// <summary>
    /// The base endpoint of the Philips Hue bridge. 
    /// </summary>
    protected readonly string BaseEndpoint;

    /// <summary>
    /// HueBridge configuration storing information on the HueBridge. 
    /// </summary>
    protected readonly HueConfiguration Configuration;

    /// <summary>
    /// Creates a new insance of a repository. 
    /// </summary>
    /// <param name="configPath">The path to the HueBridgeConfiguration compatible JSON file.</param>
    public BaseHueRepository(string configPath) : this(HueConfiguration.FromJson(configPath)) { }

    /// <summary>
    /// Creates a new instance of a repository. 
    /// </summary>
    /// <param name="config">The configuration data of a HueBridge.</param>
    public BaseHueRepository(HueConfiguration config)
    {
        BaseEndpoint = $"https://{config.Ip}/clip/v2";
        Configuration = config;
    }

        /// <summary>
    /// Builds a HTTP client to be used for calls to a HueBridge. 
    /// </summary>
    /// <returns>A HttpClient to be used for calls to a HueBridge.</returns>
    protected HttpClient BuildHttpClient()
    {
        var handler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
        };
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("hue-application-key", Configuration.AppKey);
        return client;
    }
}