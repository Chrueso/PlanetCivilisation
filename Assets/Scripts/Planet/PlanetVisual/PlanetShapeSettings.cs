using UnityEngine;

//[CreateAssetMenu(fileName = "New Planet Shape Settings", menuName = "Planet Settings/Planet Shape Settings")]

[System.Serializable]
public class PlanetShapeSettings 
{
    public float PlanetRadius = 1f;
    public NoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool IsEnabled = true;
        public bool UseFirstLayerAsMask;
        public PlanetNoiseSettings NoiseSettings;

        public NoiseLayer(bool isEnabled, bool useFirstLayerAsMask, PlanetNoiseSettings noiseSettings)
        {
            IsEnabled = isEnabled;
            UseFirstLayerAsMask = useFirstLayerAsMask;
            NoiseSettings = noiseSettings;
        }
    }

    public PlanetShapeSettings(float planetRadius, NoiseLayer[] noiseLayers)
    {
        PlanetRadius = planetRadius;
        NoiseLayers = noiseLayers;
    }
}
