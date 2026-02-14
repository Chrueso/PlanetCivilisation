using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public FactionType FactionType { get; private set; }
    public int Structures { get; private set; }
    public int Population { get; private set; }
    [SerializeField] private PlanetVisual planetVisual;

    public void Init(PlanetData data)
    {
        PlanetName = data.Name;

        // Assign initial resources
        Resources.Add(ResourceType.Rock, 0); // every planet has rock by default

        // Assign the randomly generated resource type
        foreach (var resource in data.ResourceTypes)
        {
            Resources.Add(resource, 0);
        }

        FactionType = data.FactionType;

        planetVisual.GeneratePlanetVisual(data.ShapeSettings, data.ColorSettings);
    }

    //public Planet(string planetName, List<ResourceType> additionalResources, FactionType factionType)
    //{
    //    PlanetName = planetName;

    //    // Assign initial resources
    //    Resources.Add(ResourceType.Rock, 0); // every planet has rock by default

    //    // Assign the randomly generated resource type
    //    foreach (var resource in additionalResources)
    //    {
    //        Resources.Add(resource, 0);
    //    }

    //    FactionType = factionType;
    //}

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

    //public Planet(string name, List<Resource> resources, Structure structure)
    //{
    //    this.name = name;
    //    this.resources = resources;
    //    this.structure = structure;
    //}

    public void GainResourcePerTurn() 
    {
        // Add resource to player data
    }

    public void GainStructureBenefit()
    {
        // Based on structure call functions to stuff
    }

    private void IncreasePopulation()
    {

    }

    private void GetRarerOre()
    {

    }

    private void CreateShip()
    {

    }
}
