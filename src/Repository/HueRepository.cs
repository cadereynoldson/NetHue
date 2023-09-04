namespace NetHue;

using System.Text;
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
    /// HueBridge configuration storing information on the HueBridge. 
    /// </summary>
    private readonly HueBridgeConfiguration Configuration;

    /// <summary>
    /// Creates a new insance of a HueRepository. 
    /// </summary>
    /// <param name="configPath">The path to the HueBridgeConfiguration compatible JSON file.</param>
    public HueRepository(string configPath) : this(HueBridgeConfiguration.FromFile(configPath)) { }

    /// <summary>
    /// Creates a new instance of a HueRepository. 
    /// </summary>
    /// <param name="config">The configuration data of a HueBridge.</param>
    public HueRepository(HueBridgeConfiguration config)
    {
        BaseEndpoint = $"https://{config.Ip}/clip/v2";
        Configuration = config;
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
            using HttpClient client = BuildHttpClient();

            var endpoint = $"{BaseEndpoint}/{path}";
            HttpResponseMessage response = await client.GetAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            var errors = ParseErrors(responseContent);

            if (response.IsSuccessStatusCode && errors.Length == 0)
            {
                return responseContent;
            }
            else
            {
                throw BuildErrorException(ref response, "GET", responseContent);
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
    /// Sends a PUT request to the Hue API endpoint. 
    /// </summary>
    /// <param name="path">The path of the API endpoint to request.</param>
    /// <param name="jsonBody">The JSON formatted body to send in this request.</param>
    /// <returns>A Task representing the asynchronous operation, returning the response body as a string.</returns>
    /// <exception cref="HueHttpException">Thrown when the HTTP request fails, returns a non-success status code, or contains errors in the response.</exception>
    public async Task<string> Put(string path, string jsonBody)
    {
        try
        {
            using HttpClient client = BuildHttpClient();

            var endpoint = $"{BaseEndpoint}/{path}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var errors = ParseErrors(responseContent);

            if (response.IsSuccessStatusCode && errors.Length == 0)
            {
                return responseContent;
            }
            else
            {
                throw BuildErrorException(ref response, "PUT", responseContent);
            }
        }
        catch (Exception ex)
        {
            throw new HueHttpException(ex.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="method"></param>
    /// <param name="responseContent"></param>
    /// <returns></returns>
    private HueHttpException BuildErrorException(ref HttpResponseMessage response, string method, string responseContent)
    {
        var messageHeader = response.IsSuccessStatusCode ?
                    $"HueRepository.{method}() succeeded with status code {response.StatusCode}, but errors exist in response" :
                    $"HueRepository.{method}() failed with status code: {response.StatusCode}";

        return new HueHttpException(
            $"{messageHeader}: {ParseErrors(responseContent)}",
            response: responseContent
        );
    }

    /// <summary>
    /// Builds a HTTP client to be used for calls to a HueBridge. 
    /// </summary>
    /// <returns>A HttpClient to be used for calls to a HueBridge.</returns>
    private HttpClient BuildHttpClient()
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