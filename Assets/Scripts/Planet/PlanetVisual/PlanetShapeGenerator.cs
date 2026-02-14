using UnityEngine;

public class PlanetShapeGenerator 
{
    private PlanetShapeSettings settings;
    private INoiseFilter[] noiseFilters;
    public MinMaxf ElevationMinMax;

    public void UpdateSettings(PlanetShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.NoiseLayers.Length];

        for(int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.NoiseLayers[i].NoiseSettings);
            ElevationMinMax = new MinMaxf();
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);

            if (settings.NoiseLayers[0].IsEnabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.NoiseLayers[i].IsEnabled)
            {
                float mask = (settings.NoiseLayers[i].UseFirstLayerAsMask) ? firstLayerValue : 1; // mask makes it so that the first layer controls the influence of the other layers
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }   
        elevation = settings.PlanetRadius * (1 + elevation);
        ElevationMinMax.AddValue(elevation); // keeps track of min elevation and max elevation of all generated vertices
        return pointOnUnitSphere * elevation;
    }   
}
