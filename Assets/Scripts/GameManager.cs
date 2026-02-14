using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Galaxy galaxyPrefab;

    public Galaxy CurrentGalaxy { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random GalaxyRNG { get; private set; }
    public System.Random PlanetRNG { get; private set; } // Maybe il change to data struct which combines all rng needed later -Zen

    private void Start()
    {
        // First generate galaxy name which is the seed for everything
        string galaxyName = GalaxyGenerator.GenerateGalaxyName();
        SeedInt = SeedUtil.StringToHashCode(galaxyName);

        // Then using the galaxy seed to generate seeds for the things in game ask me if need clarity -Zen
        // MoonRNG etc...
        // Basically anything that needs a seed for generation in game should use this pattern
        GalaxyRNG = new System.Random(SeedInt);
        PlanetRNG = new System.Random(SeedInt + 1000);


        CurrentGalaxy = Instantiate(galaxyPrefab);
        CurrentGalaxy.Init(galaxyName, GalaxyRNG);

        Debug.Log($"Generated Galaxy Name: {CurrentGalaxy.GalaxyName} with Seed: {SeedInt}");

        Debug.Log("Planets in Galaxy:");
        foreach (var planet in CurrentGalaxy.AllPlanets)
        {
            // Join all resources with comma
            string resources = string.Join(", ", planet.Resources.Keys);

            // Print planet name, resources, and extra newline
            Debug.Log($"{planet.PlanetName}\n{resources}\n{planet.FactionType}\n");

        }
    }

}
