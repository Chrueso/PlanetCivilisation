using UnityEngine;

public class PlanetShapeGenerator 
{
    private PlanetShapeSettingsSO settings;
    private NoiseFilter noiseFilter;

    public PlanetShapeGenerator(PlanetShapeSettingsSO settings)
    {
        this.settings = settings;
        noiseFilter = new NoiseFilter();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float elevation = noiseFilter.Evaluate(pointOnUnitSphere);
        return pointOnUnitSphere * settings.PlanetRadius * (1 + elevation);
    }   
}
