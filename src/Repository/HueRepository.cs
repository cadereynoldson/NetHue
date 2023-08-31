namespace Hue;

using System.Text.Json;


/// <summary>
/// Handles fetching data from a Phillips Hue Bridge. 
/// </summary>
public class HueRepository
{

    /// <summary>
    /// The base endpoint of the Philips Hue bridge. 
    /// </summary>
    private readonly string BaseEndpoint;

    /// <summary>
    /// The HTTP client we will use to make HTTP requests to the Philips Hue Bridge. 
    /// </summary>
    private readonly HttpClient Client;

    /// <summary>
    /// Creates a new insance of a HueRepository. 
    /// </summary>
    /// <param name="configPath">The path to the HueBridgeConfiguration compatible JSON file.</param>
    public HueRepository(string configPath) : this(HueBridgeConfiguration.FromFile(configPath)) {}

    /// <summary>
    /// Creates a new instance of a HueRepository. 
    /// </summary>
    /// <param name="config">The configuration data of a HueBridge.</param>
    public HueRepository(HueBridgeConfiguration config)
    {
        BaseEndpoint = $"https://{config.Ip}/clip/v2";

        // Workaround for the meantime to allow for ignoring SSL.
        // Note, this is because old hue bridges have a self signed certificate
        // and take some custom certificate checking which isn't implemented yet. 
        var httpClientHandler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            }
        };

        Client = new HttpClient(httpClientHandler);
        Client.DefaultRequestHeaders.Add("hue-application-key", config.AppKey);

    }

    /// <summary>
    /// Sends a GET request to the Hue API endpoint.
    /// </summary>
    /// <param name="path">The path of the API endpoint to request.</param>
    /// <returns>A Task representing the asynchronous operation, returning the response body as a string.</returns>
    /// <exception cref="HueHttpException">Thrown when the HTTP request fails, returns a non-success status code, or contains errors in the response.</exception>
    public async Task<string> Get(string path)
    {
        try
        {
            var endpoint = $"{BaseEndpoint}/{path}";
            HttpResponseMessage response = await Client.GetAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            var errors = ParseErrors(responseContent);

            if (response.IsSuccessStatusCode && errors.Count() != 0)
            {
                // Only return successful response when errors are not contained. 
                return responseContent; 
            }
            else
            {
                // The request was not successful, handle the error
                var messageHeader = response.IsSuccessStatusCode ?
                    $"HueRepository.GET() succeeded with status code {response.StatusCode}, but errors exist in response" :
                    $"HueRepository.GET() failed with status code: {response.StatusCode}";

                throw new HueHttpException(
                    $"{messageHeader}: {ParseErrors(responseContent)}",
                    response: responseContent
                );
            }
        }
        catch (Exception ex)
        {
            throw new HueHttpException(
                ex.Message
            );
        }
    }

    /// <summary>
    /// Parses a list of errors from a JSON object top-level containing an "errors" array property. 
    /// Will raise an error if there is no "errors" array contained in the JSON object. 
    /// </summary>
    /// <param name="json">The JSON element to parse errors from. </param>
    /// <returns>A string containing zero or more errors.</returns>
    public static string ParseErrors(JsonDocument json)
    {
        return ParseErrors(json.RootElement);
    }

    /// <summary>
    /// Parses a list of errors from a JSON formatted string, with the top level containing an "errors" array property. 
    /// Will raise an error if there is no "errors" array contained in the JSON object. 
    /// </summary>
    /// <param name="json">The JSON formatted string to parse errors from. </param>
    /// <returns>A string containing zero or more errors.</returns>    
    public static string ParseErrors(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        return ParseErrors(document);
    }

    /// <summary>
    /// Parses a list of errors from a JSON object top-level containing an "errors" array property. 
    /// Will raise an error if there is no "errors" array contained in the JSON object. 
    /// </summary>
    /// <param name="json">The JSON element to parse errors from. </param>
    /// <returns>A string containing zero or more errors.</returns>
    public static string ParseErrors(JsonElement json)
    {
        var str = ""; 
        var errorCount = 0;

        foreach (JsonElement errorData in json.GetProperty("errors").EnumerateArray())
        {
            var errorMessage = errorData.GetProperty("description");
            str += $"<{++errorCount}>: {errorMessage}";
        }

        return str; 
    }
}