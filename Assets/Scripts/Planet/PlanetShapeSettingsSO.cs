using UnityEngine;

[CreateAssetMenu(fileName = "New Planet Shape Settings", menuName = "Planet Settings/Planet Shape Settings")]
public class PlanetShapeSettingsSO : ScriptableObject
{
    public float PlanetRadius = 1f;
    public NoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool IsEnabled = true;
        public bool UseFirstLayerAsMask;
        public PlanetNoiseSettings NoiseSettings;
    }
}
