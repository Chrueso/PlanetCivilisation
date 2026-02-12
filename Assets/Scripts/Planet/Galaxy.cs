using System.Collections.Generic;
using UnityEngine;

public class Galaxy
{
    public string GalaxyName { get; private set; }
    public List<PlanetData> Planets = new List<PlanetData>();

    // Constructor
    public Galaxy(string galaxyName, List<PlanetData> planets)
    {
        GalaxyName = galaxyName;
        Planets = planets;
    }
}
