using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace NetHue;
/// <summary>
/// Class which handles making HTTP requests to a hue bridge on the network this device is connected to.
/// </summary>
public class HueHttpHelper
{

    /// <summary>
    /// The base endpoint of the Philips Hue bridge. 
    /// </summary>
    private readonly string BaseEndpoint;

    /// <summary>
    /// The HTTP client we will use to make HTTP requests to the Philips Hue Bridge. 
    /// </summary>
    private readonly HttpClient Client;

    public HueHttpHelper(string ip, string appKey)
    {
        BaseEndpoint = $"https://{ip}/clip/v2";

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
        Client.DefaultRequestHeaders.Add("hue-application-key", appKey);

    }

    /// <summary>
    /// Sends a GET request to the Hue API endpoint.
    /// </summary>
    /// <param name="path">The path of the API endpoint to request.</param>
    /// <returns>A Task representing the asynchronous operation, returning the response body as a string.</returns>
    /// <exception cref="HueHttpException">Thrown when the HTTP request fails or returns a non-success status code.</exception>
    public async Task<string> Get(string path)
    {
        try
        {
            var endpoint = $"{this.BaseEndpoint}/{path}";
            HttpResponseMessage response = await this.Client.GetAsync(endpoint);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return responseContent; 
            }
            else
            {
                // The request was not successful, handle the error
                throw new HueHttpException(
                    $"HueHttpHelper.GET() failed with status code: {response.StatusCode}: {ParseErrors(responseContent)}",
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