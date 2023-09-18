namespace NetHue;

using System.Text;
using System.Text.Json;


/// <summary>
/// Handles fetching data from a Phillips Hue Bridge. 
/// </summary>
public class HueRepository : BaseHueRepository
{

    /// <inheritdoc/>
    public HueRepository(string configPath) : base(configPath) { }

    /// <inheritdoc/>
    public HueRepository(HueConfiguration config) : base(config) { }

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
    /// Builds an exception containing the errors of a response from the bridge.
    /// </summary>
    /// <param name="response">The response containing the errors</param>
    /// <param name="method">The method in which this error occured. (GET/PUT/etc.)</param>
    /// <param name="responseContent">The content of the response.</param>
    /// <returns>HueHttpException containing the errors.</returns>
    private static HueHttpException BuildErrorException(ref HttpResponseMessage response, string method, string responseContent)
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
        var errors = new List<string>();
        var errorCount = 0;

        foreach (JsonElement errorData in json.GetProperty("errors").EnumerateArray())
        {
            var errorMessage = errorData.GetProperty("description");

            errors.Add($"{{<{++errorCount}>: {errorMessage}}}");
        }

        return string.Join(", ", errors);
    }
}