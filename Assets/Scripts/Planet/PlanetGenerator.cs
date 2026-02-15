using System.Collections.Generic;
using UnityEngine;
using static PlanetShapeSettings;

public class PlanetGenerator : Singleton<PlanetGenerator>
{
    [SerializeField] private Planet planetPrefab;
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
    private string GeneratePlanetName(System.Random rng)
    {
        string baseName = planetBaseNames[rng.Next(planetBaseNames.Length)];
        char[] suffix = new char[4];
        for (int i = 0; i < suffix.Length; i++)
            suffix[i] = GalaxyGenerator.SuffixChar[rng.Next(GalaxyGenerator.SuffixChar.Length)];
        return baseName + "-" + new string(suffix);
    }

    private List<ResourceType> GenerateResourceTypes(System.Random rng) // should use the plannet name as seed
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
    private FactionType PickFactionType(System.Random rng) // should use the plannet name as seed
    {
        var values = System.Enum.GetValues(typeof(FactionType));

        // Pick a random index
        int index = rng.Next(values.Length);

        // Return the enum value
        return (FactionType)values.GetValue(index);
    }

    [SerializeField] PlanetVisualProcederalPresetSO preset;

    private PlanetNoiseSettings GenerateNoiseSettings(System.Random rng, PlanetNoiseSettingsPreset presetNoiseSettings)
    {
        PlanetNoiseSettings noiseSettings = new PlanetNoiseSettings();
        noiseSettings.FilterType = presetNoiseSettings.FilterType;

        var s = presetNoiseSettings.SimpleNoiseSettingsPreset;

        float strength = RandomUtil.NextRangef(rng, s.Strength.Min, s.Strength.Max);
        int layers = rng.Next(s.Layers.Min, s.Layers.Max + 1);
        float persistence = RandomUtil.NextRangef(rng, s.Persistence.Min, s.Persistence.Max);
        float baseRoughness = RandomUtil.NextRangef(rng, s.BaseRoughness.Min, s.BaseRoughness.Max);
        float roughness = RandomUtil.NextRangef(rng, s.Roughness.Min, s.Roughness.Max);
        Vector3 center = new Vector3((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble());
        float minValue = RandomUtil.NextRangef(rng, s.MinValue.Min, s.MinValue.Max);

        switch (presetNoiseSettings.FilterType)
        {
            case PlanetNoiseSettings.NoiseFilterType.Simple:
                noiseSettings.SimpleNoiseSettings =
                new PlanetNoiseSettings.PlanetSimpleNoiseSettings(
                    strength,
                    layers,
                    persistence,
                    baseRoughness,
                    roughness,
                    center,
                    minValue
                );
                break;

            case PlanetNoiseSettings.NoiseFilterType.Rigid:
                var r = presetNoiseSettings.RigidNoiseSettingsPreset;
                float weightMultiplier = RandomUtil.NextRangef(rng, r.WeightMultiplier.Min, r.WeightMultiplier.Max);

                noiseSettings.RigidNoiseSettings =
                new PlanetNoiseSettings.PlanetRigidNoiseSettings(
                    strength,
                    layers,
                    persistence,
                    baseRoughness,
                    roughness,
                    center,
                    minValue,
                    weightMultiplier
                );
                break;
        }

        return noiseSettings;
}

    private PlanetShapeSettings GeneratePlanetShapeSettings(System.Random rng)
    {
        float planetRadius = RandomUtil.NextRangef(rng, preset.PlanetRadius.Min, preset.PlanetRadius.Max);

        PlanetNoiseLayerPreset[] noiseLayerPresets = preset.NoiseLayersPresets;
        PlanetNoiseLayer[] noiseLayers = new PlanetNoiseLayer[preset.NoiseLayersPresets.Length];

        for (int i = 0; i < noiseLayers.Length; i++)
        {
            PlanetNoiseLayerPreset noiseLayerPreset = noiseLayerPresets[i];
            PlanetNoiseSettings noiseSettings = GenerateNoiseSettings(rng, noiseLayerPreset.NoiseSettingsPreset);

            noiseLayers[i] = new PlanetNoiseLayer(noiseLayerPreset.IsEnabled, noiseLayerPreset.UseFirstLayerAsMask, noiseSettings);
        }

        return new PlanetShapeSettings(planetRadius, noiseLayers);
    }

    private PlanetColorSettings GeneratePlanetColorSettings(System.Random rng)
    {
        //Make like base color for each graddient key then a min max for randomize hue sat 
        Material material = preset.Material;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            preset.Gradient.colorKeys,
            preset.Gradient.alphaKeys
        );

        GradientColorKey[] colorKeys = gradient.colorKeys;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

        for (int i = 0; i < colorKeys.Length; i++)
        {
            Color.RGBToHSV(colorKeys[i].color, out float h, out float s, out float v);

            h += RandomUtil.NextRangef(rng, -0.1f, 0.1f);
            s *= RandomUtil.NextRangef(rng, 0.9f, 1.1f);
            v *= RandomUtil.NextRangef(rng, 0.9f, 1.1f);

            h = h % 1f;       // wraps above 1
            if (h < 0f) h += 1f; // wraps negative values

            s = Mathf.Clamp01(s);
            v = Mathf.Clamp01(v);

            colorKeys[i].color = Color.HSVToRGB(h, s, v);
        }

        gradient.SetKeys(colorKeys, alphaKeys);

        return new PlanetColorSettings(gradient, material);
    }

    private PlanetData GeneratePlanetData(System.Random rng)
    {
        string planetName = GeneratePlanetName(rng);

        List<ResourceType> additionalResources = GenerateResourceTypes(rng);

        FactionType factionType = PickFactionType(rng);

        PlanetShapeSettings shapeSettings = GeneratePlanetShapeSettings(rng);

        PlanetColorSettings colorSettings = GeneratePlanetColorSettings(rng);

        return new PlanetData(planetName, additionalResources, factionType, shapeSettings, colorSettings);

    }

    public Planet InstantiatePlanet(System.Random rng, Vector3 position, Transform parent)
    {
        Planet planet = Instantiate(planetPrefab, position, Quaternion.identity, parent);
        PlanetData data = GeneratePlanetData(rng);

        planet.Init(data);

        return planet;

    }

}
