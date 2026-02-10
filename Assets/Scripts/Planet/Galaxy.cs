using System.Collections.Generic;
using UnityEngine;

public class Galaxy
{
    public string GalaxyName { get; private set; }
    public List<Planet> Planets = new List<Planet>();

    // Constructor
    public Galaxy(string galaxyName, List<Planet> planets)
    {
        GalaxyName = galaxyName;
        Planets = planets;
    }
}
