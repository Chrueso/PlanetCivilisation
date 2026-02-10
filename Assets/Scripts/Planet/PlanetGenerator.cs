using System.Collections.Generic;

public static class PlanetGenerator 
{
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

    // We use planet names as a seed to generate aa unique planet visual/ resource/ etc
    private static string GeneratePlanetName(System.Random rng)
    {
        string baseName = planetBaseNames[rng.Next(planetBaseNames.Length)];
        char[] suffix = new char[4];
        for (int i = 0; i < suffix.Length; i++)
            suffix[i] = GalaxyGenerator.SuffixChar[rng.Next(GalaxyGenerator.SuffixChar.Length)];
        return baseName + "-" + new string(suffix);
    }

    private static List<ResourceType> GenerateAdditionalResource(System.Random rng) // should use the plannet name as seed
    {
        int count = rng.Next(0, 2); // Each planet has 0 to 2 additional resources
        List<ResourceType> types = new List<ResourceType>();

        if (count == 0)
        {
            return types; // No additional resources
        }
        else if (count == 2)
        {
            types.Add(ResourceType.Copper);
            types.Add(ResourceType.Iron);
        }
        else if (count == 1)
        {
            int num = rng.Next(0, 1);

            if (num == 0) types.Add(ResourceType.Copper);
            else if (num == 1) types.Add(ResourceType.Iron);
        }

        return types;
    }

    // Randomize faction type this is temp change later idk wtf factions do
    private static FactionType PickFactionType(System.Random rng) // should use the plannet name as seed
    {
        var values = System.Enum.GetValues(typeof(FactionType));

        // Pick a random index
        int index = rng.Next(values.Length);

        // Return the enum value
        return (FactionType)values.GetValue(index);
    }

    public static Planet GeneratePlanet(System.Random rng)
    {
        string planetName = GeneratePlanetName(rng);

        List<ResourceType> additionalResources = GenerateAdditionalResource(rng);

        FactionType factionType = PickFactionType(rng);

        return new Planet(planetName, additionalResources, factionType);
    }
}
