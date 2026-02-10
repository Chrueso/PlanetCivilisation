using System.Collections.Generic;

public static class GalaxyGenerator 
{
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

    public static readonly string SuffixChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    // We use galaxy names as a seed to generate the galaxy which determines the map layout and the planets and other things generated
    public static string GenerateGalaxyName()
    {
        System.Random rng = new System.Random();

        string baseName = galaxyBaseNames[rng.Next(galaxyBaseNames.Length)];

        char[] suffix = new char[3];
        for (int i = 0; i < suffix.Length; i++)
            suffix[i] = SuffixChar[rng.Next(SuffixChar.Length)];
        return baseName + "-" + new string(suffix);
    }

    public static Galaxy GenerateGalaxy(string galaxyName, int planetAmount, System.Random rng)
    {
        List<Planet> planets = new List<Planet>();

        for (int i = 0; i < planetAmount; i++)
        {
            Planet planet = PlanetGenerator.GeneratePlanet(rng);
            planets.Add(planet);
        }

        return new Galaxy(galaxyName, planets);
    }



    //// Debug
    //[ContextMenu("Generate galaxy name")]
    //private void PrintGeneratedGalaxyName()
    //{
    //    string planetName = GenerateGalaxyName();
    //    Debug.Log(planetName);
    //}

    //[ContextMenu("Generate planet name")]
    //private void PrintGeneratedPlanetName()
    //{
    //    string planetName = GeneratePlanetName();   
    //    Debug.Log(planetName);
    //}
}
