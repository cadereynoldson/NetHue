namespace NetHue;

/// <summary>
/// A repository like class which is designed to manage the states of existing HueResources.
/// Use this class in tandem with HueEventHandler to keep constant updates of your HueResources.
/// <br/>This will only UPDATE HueResources, does not handle new resources being created.
/// <br/>Currently supported resources: HueLight, HueScene, HueLightGroup
/// <br/> NOTE: HueLightGroup events are much slower than HueLight and scene events, may take a while for them to update. 
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
    /// Creates a new instance of a HueEventHandler.
    /// <br/>Currently supported resources: HueLight, HueScene, HueLightGroup
    /// <br/> NOTE: HueLightGroup events are much slower than HueLight and scene events, may take a while for them to update. 
    /// </summary>
    public HueResourceManager()
    {
        Resources = new Dictionary<string, HueResource>();
    }

    /// <summary>
    /// Adds a new HueResource for this event handler to manage. 
    /// </summary>
    /// <param name="resource">The new resource to manage</param>
    /// <returns>This HueResourceManager</returns>
    public HueResourceManager Manage(HueResource resource)
    {
        lock (ThreadLock)
        {
            Resources.Add(resource.Id, resource);
        }
        return this;
    }

    /// <summary>
    /// Adds new HueResources for this event handler to manage. 
    /// </summary>
    /// <param name="resources">The new resources to manage</param>
    /// <returns>This HueResourceManager</returns>
    public HueResourceManager Manage(List<HueResource> resources)
    {
        lock (ThreadLock)
        {
            foreach (var resource in resources)
            {
                Manage(resource);
            }
        }
        return this;
    }

    /// <summary>
    /// Manages all of the lights contained in the parameterized list.
    /// </summary>
    /// <param name="lights">The lights to manage</param>
    /// <returns>This HueResourceManager</returns>
    public HueResourceManager Manage(List<HueLight> lights)
    {
        return Manage(lights.Select(l => (HueResource) l).ToList());
    }

    /// <summary>
    /// Manages all of the scenes contained in the parameterized list.
    /// </summary>
    /// <param name="scenes">The scenes to manage</param>
    /// <returns>This HueResourceManager</returns>
    public HueResourceManager Manage(List<HueScene> scenes)
    {
        return Manage(scenes.Select(s => (HueResource) s).ToList());
    }

    /// <summary>
    /// Applies any new "update" resource events to the resources we are managing. 
    /// </summary>
    /// <param name="events">The events on the HueBridge</param>
    public void Apply(List<HueResourceEvent> events)
    {
        lock (ThreadLock)
        {
            foreach (HueResourceEvent resourceEvent in events)
            {
                if (Resources.ContainsKey(resourceEvent.ResourceId))
                {
                    resourceEvent.Apply(Resources[resourceEvent.ResourceId]);
                }
            }
        }
    }
}