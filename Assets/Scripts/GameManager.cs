using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random GalaxyRNG { get; private set; }
    public System.Random PlanetRNG { get; private set; } // Maybe il change to data struct which combines all rng needed later -Zen

    private void Start()
    {
        GenerateSeed();
        PlanetManager.Instance.GeneratePlanets(PlanetRNG);
    }

    private void GenerateSeed()
    {
        // First generate galaxy name which is the seed for everything
        GalaxyName = GalaxyGenerator.GenerateGalaxyName();
        SeedInt = SeedUtil.StringToHashCode(GalaxyName);

        // Then using the galaxy seed to generate seeds for the things in game ask me if need clarity -Zen
        // MoonRNG etc...
        // Basically anything that needs a seed for generation in game should use this pattern
        GalaxyRNG = new System.Random(SeedInt);
        PlanetRNG = new System.Random(SeedInt + 1000);

        Debug.Log($"Generated Galaxy Name: {GalaxyName} with Seed: {SeedInt}");
    }

}
