using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int planetAmount = 50;

    public Galaxy CurrentGalaxy { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random PlanetRNG { get; private set; } // Maybe il change to data struct which combines all rng needed later -Zen

    [SerializeField] private List<Planet> allPlanets = new List<Planet>();
    public List<Planet> PlanetsWithFactions { get; private set; } = new List<Planet>();

    protected override void Awake()
    {
        base.Awake();

        if (TurnManager.Instance == null)
        {
            GameObject tmObj = new GameObject("TurnManager");
            tmObj.AddComponent<TurnManager>();
        }
    }

    private void Start()
    {
        // First generate galaxy name which is the seed for everything
        string galaxyName = GalaxyGenerator.GenerateGalaxyName();
        SeedInt = SeedUtil.StringToHashCode(galaxyName);

        // Then using the galaxy seed to generate seeds for the things in game ask me if need clarity -Zen
        PlanetRNG = new System.Random(SeedInt);
        // MoonNameRNG etc...
        // Basically anything that needs a seed for generation in game should use this pattern

        // Then generate the galaxy
        CurrentGalaxy = GalaxyGenerator.GenerateGalaxy(galaxyName, planetAmount, PlanetRNG);

        Debug.Log($"Generated Galaxy Name: {CurrentGalaxy.GalaxyName} with Seed: {SeedInt}");

        allPlanets = CurrentGalaxy.Planets;
        PlanetsWithFactions = allPlanets.FindAll(planet => planet.FactionType != FactionType.Nothing);

        Debug.Log("Planets in Galaxy:");
        foreach (var planet in CurrentGalaxy.Planets)
        {
            // Join all resources with comma
            string resources = string.Join(", ", planet.Resources.Keys);

            // Print planet name, resources, and extra newline
            Debug.Log($"{planet.PlanetName}\n{resources}\n{planet.FactionType}\n");

        }
    }

}
