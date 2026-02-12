using System.Collections.Generic;
using UnityEngine;

public class Planet //Should be planet data cause u generate a base planet prefab and then assign data to it
{
    public string PlanetName { get; private set; }  
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public int Structures { get; private set; }
    public FactionType FactionType { get; private set; }

    // Constructor
    public Planet(string planetName, List<ResourceType> additionalResources, FactionType factionType)
    {
        PlanetName = planetName;

        // Assign initial resources
        Resources.Add(ResourceType.Rock, 0); // every planet has rock by default

        // Assign the randomly generated resource type
        foreach (var resource in additionalResources)
        {
            Resources.Add(resource, 0);
        }

        FactionType = factionType;
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
    private string name;
    private int population;
    private List<Resource> resources;
    private Structure structure;

    public Planet(string name, List<Resource> resources, Structure structure)
    {
        this.name = name;
        this.resources = resources;
        this.structure = structure;
    }

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
