namespace Hue;
class HueHttpException : Exception {

    public string Response { get; init; } 

    public HueHttpException(
        string message,
        string response = ""
    ) : base(message) 
    {
        this.Response = response;
    }
}