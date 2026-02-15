using UnityEngine;

[System.Serializable]
public class PlanetShapeSettings 
{
    public float PlanetRadius = 1f;
    public PlanetNoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class PlanetNoiseLayer
    {
        public bool IsEnabled = true;
        public bool UseFirstLayerAsMask;
        public PlanetNoiseSettings NoiseSettings;

        public PlanetNoiseLayer(bool isEnabled, bool useFirstLayerAsMask, PlanetNoiseSettings noiseSettings)
        {
            IsEnabled = isEnabled;
            UseFirstLayerAsMask = useFirstLayerAsMask;
            NoiseSettings = noiseSettings;
        }
    }

    public PlanetShapeSettings() { }

    public PlanetShapeSettings(float planetRadius, PlanetNoiseLayer[] noiseLayers)
    {
        PlanetRadius = planetRadius;
        NoiseLayers = noiseLayers;
    }
}
