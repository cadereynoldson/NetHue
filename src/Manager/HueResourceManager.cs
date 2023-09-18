namespace NetHue;

/// <summary>
/// A repository like class which is designed to manage the states of existing HueResources.
/// Use this class in tandem with HueEventHandler to keep constant updates of your HueResources.
/// </summary>
public class HueResourceManager
{
    /// <summary>
    /// Dictionary of HueResources mapped by their ID. 
    /// </summary>
    private readonly Dictionary<string, HueResource> Resources;

    /// <summary>
    /// Lock for threading, ensures we don't update the managed resources from multiple threads at once.
    /// </summary>
    private readonly object ThreadLock = new();

    /// <summary>
    /// Creates a new instance of a HueEventHandler
    /// </summary>
    /// <param name="resources">The resources this handler will manage</param>
    public HueResourceManager(List<HueResource> resources)
    {
        Resources = new Dictionary<string, HueResource>();
        Manage(resources);
    }

    /// <summary>
    /// Adds a new HueResource for this event handler to manage. 
    /// </summary>
    /// <param name="resource">The new resource to manage</param>
    public void Manage(HueResource resource)
    {
        lock (ThreadLock)
        {
            Resources.Add(resource.Id, resource);
        }
    }

    /// <summary>
    /// Adds new HueResources for this event handler to manage. 
    /// </summary>
    /// <param name="resources">The new resources to manage</param>
    public void Manage(List<HueResource> resources)
    {
        lock (ThreadLock)
        {
            foreach (var resource in resources)
            {
                Manage(resource);
            }
        }
    }

    /// <summary>
    /// Applies any new "update" resource events to the resources we are managing. 
    /// </summary>
    /// <param name="events">The events on the HueBridge</param>
    public void Apply(List<HueResourceEvent> events)
    {
        foreach (HueResourceEvent resourceEvent in events)
        {
            if (Resources.ContainsKey(resourceEvent.ResourceId))
            {
                resourceEvent.Apply(Resources[resourceEvent.ResourceId]);
                Console.WriteLine($"Light update: {Resources[resourceEvent.ResourceId]}");
            }
        }
    }
}