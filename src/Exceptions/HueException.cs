namespace NetHue;

/// <summary>
/// Exception thrown when an invalid configuration was created for a HueBridge. 
/// </summary>
public class HueBridgeConfigurationException : Exception
{

    /// <summary>
    /// Creates an exception thrown when an invalid configuration was created for a HueBridge.
    /// </summary>
    /// <param name="message"></param>
    public HueBridgeConfigurationException(string message) : base(message) {}
}