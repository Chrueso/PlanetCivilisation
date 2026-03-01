using System.Collections.Generic;

public class PlanetData 
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public FactionType FactionType { get; private set; }
    public int Structures { get; private set; }

    public PlanetData(string planetName, List<ResourceType> resourceTypes, FactionType factionType)
    {
        PlanetName = planetName;

        // Assign initial resources
        Resources.Add(ResourceType.Rock, 0); // every planet has rock by default not sure if still the case?

        // Assign the randomly generated resource type
        foreach (var resource in resourceTypes)
        {
            Resources.Add(resource, 0);
        }

        FactionType = factionType;
    }

    public void DebugPurposes()
    {
        Structures++;
    }

    //Idk what is this for yet so i comment -Zen
    //private void IncreaseResource()
    //{
    //    if (Resources.Count > 0)
    //    {
    //        foreach (var resource in Resources.Keys)
    //        {
    //            Resources[resource] += 1;
    //        }
    //    }
    //}

    //public void GainResourcePerTurn() 
    //{
    //    // Add resource to player data
    //}

    //public void GainStructureBenefit()
    //{
    //    // Based on structure call functions to stuff
    //}

    //private void IncreasePopulation()
    //{

    //}

    //private void GetRarerOre()
    //{

    //}

    //private void CreateShip()
    //{

    //}
}
