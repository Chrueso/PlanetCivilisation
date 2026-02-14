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

    private List<ResourceType> GenerateAdditionalResource(System.Random rng) // should use the plannet name as seed
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

    [SerializeField] PlanetVisualPresets preset;

    private PlanetShapeSettings GeneratePlanetShapeSettings(System.Random rng)
    {
        float planetRadius = RandomUtil.NextRangef(rng, preset.PlanetRadius.Min, preset.PlanetRadius.Max);

        NoiseLayerPreset[] noiseLayerPresets = preset.NoiseLayersPresets;
        NoiseLayer[] noiseLayers = new NoiseLayer[preset.NoiseLayersPresets.Length];

        for (int i = 0; i < noiseLayers.Length; i++)
        {
            var presetLayer = noiseLayerPresets[i];
            var presetSettings = presetLayer.NoiseSettingsPreset;

            PlanetNoiseSettings finalSettings = new PlanetNoiseSettings();
            finalSettings.FilterType = presetSettings.FilterType;

            if (presetSettings.FilterType == PlanetNoiseSettings.NoiseFilterType.Simple)
            {
                var p = presetSettings.SimpleNoiseSettingsPreset;

                float strength = RandomUtil.NextRangef(rng, p.Strength.Min, p.Strength.Max);
                int layers = rng.Next(p.Layers.Min, p.Layers.Max + 1);
                float persistence = RandomUtil.NextRangef(rng, p.Persistence.Min, p.Persistence.Max);
                float baseRoughness = RandomUtil.NextRangef(rng, p.BaseRoughness.Min, p.BaseRoughness.Max);
                float roughness = RandomUtil.NextRangef(rng, p.Roughness.Min, p.Roughness.Max);
                Vector3 center = new Vector3((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble());
                float minValue = RandomUtil.NextRangef(rng, p.MinValue.Min, p.MinValue.Max);

                finalSettings.SimpleNoiseSettings =
                    new PlanetNoiseSettings.PlanetSimpleNoiseSettings(
                        strength,
                        layers,
                        persistence,
                        baseRoughness,
                        roughness,
                        center,
                        minValue
                    );
            }
            else if (presetSettings.FilterType == PlanetNoiseSettings.NoiseFilterType.Rigid)
            {
                var p = presetSettings.RigidNoiseSettingsPreset;

                float strength = RandomUtil.NextRangef(rng, p.Strength.Min, p.Strength.Max);
                int layers = rng.Next(p.Layers.Min, p.Layers.Max + 1);
                float persistence = RandomUtil.NextRangef(rng, p.Persistence.Min, p.Persistence.Max);
                float baseRoughness = RandomUtil.NextRangef(rng, p.BaseRoughness.Min, p.BaseRoughness.Max);
                float roughness = RandomUtil.NextRangef(rng, p.Roughness.Min, p.Roughness.Max);
                Vector3 center = new Vector3((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble());
                float minValue = RandomUtil.NextRangef(rng, p.MinValue.Min, p.MinValue.Max);
                float weightMultiplier = RandomUtil.NextRangef(rng, p.WeightMultiplier.Min, p.WeightMultiplier.Max);

                finalSettings.RigidNoiseSettings =
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
            }

            noiseLayers[i] = new NoiseLayer(presetLayer.IsEnabled, presetLayer.UseFirstLayerAsMask, finalSettings);
        }

        return new PlanetShapeSettings(planetRadius, noiseLayers);
    }

    private PlanetColorSettings GeneratePlanetColorSettings(System.Random rng)
    {
        Gradient gradient = preset.Gradient;
        Material material = preset.Material;

        return new PlanetColorSettings(gradient, material);
    }

    private PlanetData GeneratePlanetData(System.Random rng)
    {
        string planetName = GeneratePlanetName(rng);

        List<ResourceType> additionalResources = GenerateAdditionalResource(rng);

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
