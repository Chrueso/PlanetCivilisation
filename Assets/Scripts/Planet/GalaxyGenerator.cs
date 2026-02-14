using UnityEngine;

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
}
