using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class GalaxyGenerator : MonoBehaviour
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random PlanetNameRNG { get; private set; }

    public List<string> GeneratedPlanetNames = new List<string>();

    static readonly string[] planetBaseNames =
    {
        "Aurelia",
        "Nyx",
        "Helios",
        "Virex",
        "Eldara",
        "Kryos",
        "Zentha",
        "Orion",
        "Caldera",
        "Solara",
        "Iris",
        "Atlas"
    }; 

    static readonly string[] galaxyBaseNames =
    {
        "Andromeda",
        "Centauri",
        "Lyra",
        "Vega",
        "Altair",
        "Sirius",
        "Aether",
        "Chronos",
        "Hyperion",
        "Nexus",
        "Zerath",
        "Aurora",
        "Lumina",
        "Nebula",
        "Polaris",
        "Ionis",
    };

    static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private void Start()
    {
        GalaxyName = GenerateGalaxyName();
        SeedInt = StringToHashCode(GalaxyName);

        Debug.Log($"Generated Galaxy Name: {GalaxyName} with Seed: {SeedInt}");

        PlanetNameRNG = new System.Random(SeedInt);

    }

    // Changes strings into a consistent hash code integer
    private int StringToHashCode(string seedStr)
    {
        unchecked
        {
            long hash = 5381L; //initial prime (Bernstein's djb2)
            foreach (char c in seedStr)
                hash = ((hash << 5) + hash) + c; //Equivalent to 33 * hash + c
            return (int)(hash & 0x7FFFFFFF);
        }
    }

    bool IsValidSeed(string seed)
    {
        return Regex.IsMatch(seed, @"^[A-Za-z]{6}\d{4}$");

    }

    // We use galaxy names as a seed to generate the galaxy which determines the map layout and the planets and other things generated
    private string GenerateGalaxyName()
    {
        System.Random rng = new System.Random();

        string baseName = galaxyBaseNames[rng.Next(galaxyBaseNames.Length)];

        char[] suffix = new char[3];
        for (int i = 0; i < suffix.Length; i++)
            suffix[i] = chars[rng.Next(chars.Length)];
        return baseName + "-" + new string(suffix);
    }

    // We use planet names as a seed to generate aa unique planet visual/ resource/ etc
    private string GeneratePlanetName()
    {
        string baseName = planetBaseNames[PlanetNameRNG.Next(planetBaseNames.Length)];
        char[] suffix = new char[4];
        for (int i = 0; i < suffix.Length; i++)
            suffix[i] = chars[PlanetNameRNG.Next(chars.Length)];
        return baseName + "-" + new string(suffix);
    }


    // Debug
    [ContextMenu("Generate galaxy name")]
    private void PrintGeneratedGalaxyName()
    {
        string planetName = GenerateGalaxyName();
        Debug.Log(planetName);
    }

    [ContextMenu("Generate planet name")]
    private void PrintGeneratedPlanetName()
    {
        string planetName = GeneratePlanetName();   
        Debug.Log(planetName);
    }

}
