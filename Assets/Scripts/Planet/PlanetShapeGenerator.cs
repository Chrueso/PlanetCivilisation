using UnityEngine;

public class PlanetShapeGenerator 
{
    private PlanetShapeSettingsSO settings;   
    
    public PlanetShapeGenerator(PlanetShapeSettingsSO settings)
    {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * settings.PlanetRadius;
    }   
}
