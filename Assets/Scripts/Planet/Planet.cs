using System.Collections.Generic;

public class Planet //Should be planet data cause u generate a base planet prefab and then assign data to it
{
    public string PlanetName { get; private set; }  

    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();

    public int Structures { get; private set; }

    // Constructor
    public Planet(string planetName, List<ResourceType> additionalResources)
    {
        PlanetName = planetName;

        // Assign initial resources
        Resources.Add(ResourceType.Rock, 0); // every planet has rock by default

        // Assign the randomly generated resource type
        foreach (var resource in additionalResources)
        {
            Resources.Add(resource, 0);
        }
    }

    private void IncreaseResource()
    {
        if (Resources.Count > 0)
        {
            foreach (var resource in Resources.Keys)
            {
                Resources[resource] += 1;
            }
        }
    }

    public void PlanetResourceGain() => IncreaseResource();
}
