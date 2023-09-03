namespace Hue;

/// <summary>
/// An exception thrown when an error occuirs with a HTTP call with a HueBridge. 
/// </summary>
public class HueHttpException : Exception {

    /// <summary>
    /// The full response from the HueBridge. 
    /// </summary>
    public string Response { get; init; } 

    /// <summary>
    /// Exception thrown when an error occurs with a HTTP call with a HueBridge. 
    /// </summary>
    /// <param name="message">The base message of the exception.</param>
    /// <param name="response">The response containing the message. </param>
    public HueHttpException(
        string message,
        string response = ""
    ) : base(message) 
    {
        Response = response;
    }
}