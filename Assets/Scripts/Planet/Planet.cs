using System.Collections.Generic;
using UnityEngine;

public class Planet
{
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
