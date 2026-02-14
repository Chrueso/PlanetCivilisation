using UnityEngine;

[CreateAssetMenu(fileName = "PlanetVisualPresets", menuName = "Scriptable Objects/PlanetVisualPresets")]
public class PlanetVisualPresets : ScriptableObject
{
    public MinMaxf PlanetRadius;

    public NoiseLayerPreset[] NoiseLayersPresets;

    public Gradient Gradient;
    public Material Material;
}

[System.Serializable]
public class NoiseLayerPreset
{
    public bool IsEnabled = true;
    public bool UseFirstLayerAsMask;
    public PlanetPresetNoiseSettings NoiseSettingsPreset;
}

[System.Serializable]
public class PlanetPresetNoiseSettings
{
    public PlanetNoiseSettings.NoiseFilterType FilterType;

    [ConditionalHide("FilterType", 0)]
    public PlanetSimpleNoiseSettingsPreset SimpleNoiseSettingsPreset;

    [ConditionalHide("FilterType", 1)]
    public PlanetRigidNoiseSettingsPreset RigidNoiseSettingsPreset;

    [System.Serializable]
    public class PlanetSimpleNoiseSettingsPreset
    {
        public MinMaxf Strength;

        public MinMax Layers;

        public MinMaxf Persistence; // Amplitude change per layer if 0.5 then each layer is half the amplitude of the previous layer

        public MinMaxf BaseRoughness;
        public MinMaxf Roughness;

        //public Vector3 Center;

        public MinMaxf MinValue;
    }

    [System.Serializable]
    public class PlanetRigidNoiseSettingsPreset : PlanetSimpleNoiseSettingsPreset
    {
        public MinMaxf WeightMultiplier;
    }
}
