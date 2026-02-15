using UnityEngine;

[System.Serializable]
public class PlanetNoiseSettings
{
    public enum NoiseFilterType { Simple, Rigid };
    public NoiseFilterType FilterType;

    public PlanetSimpleNoiseSettings SimpleNoiseSettings;
    public PlanetRigidNoiseSettings RigidNoiseSettings;

    [System.Serializable]
    public class PlanetSimpleNoiseSettings
    {
        public float Strength = 1f;

        [Range(1, 8)]
        public int Layers = 1;

        public float Persistence = 0.5f;
        public float BaseRoughness = 1f;
        public float Roughness = 2f;
        public Vector3 Center;
        public float MinValue;

        // Constructor
        public PlanetSimpleNoiseSettings(
            float strength,
            int layers,
            float persistence,
            float baseRoughness,
            float roughness,
            Vector3 center,
            float minValue)
        {
            Strength = strength;
            Layers = layers;
            Persistence = persistence;
            BaseRoughness = baseRoughness;
            Roughness = roughness;
            Center = center;
            MinValue = minValue;
        }

    }

    [System.Serializable]
    public class PlanetRigidNoiseSettings : PlanetSimpleNoiseSettings
    {
        public float WeightMultiplier = 0.8f;

        // Constructor
        public PlanetRigidNoiseSettings(
            float strength,
            int layers,
            float persistence,
            float baseRoughness,
            float roughness,
            Vector3 center,
            float minValue,
            float weightMultiplier)
            : base(strength, layers, persistence, baseRoughness, roughness, center, minValue)
        {
            WeightMultiplier = weightMultiplier;
        }
    }
}