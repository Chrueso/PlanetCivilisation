using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    private SimplexNoise noise = new SimplexNoise();
    private PlanetNoiseSettings.PlanetRigidNoiseSettings settings;

    public RigidNoiseFilter(PlanetNoiseSettings.PlanetRigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.BaseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.Layers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.Center)); // Get abs value which makes it all like upside down Us
            v *= v;                                                                       // then invert so 1 to 0 which makes it sharp peaks
                                                                                          // then square it which makes it sharper

            v *= weight; // regions which start low will be lower detail compared to regions which start at higher elevation
            weight = Mathf.Clamp01(v * settings.WeightMultiplier);

            noiseValue += v * amplitude; 
            frequency *= settings.Roughness; // if roughness is greateer than 1 frequency increases each layer
            amplitude *= settings.Persistence; // if persistence is less than 1 amplitude decreases each layer
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.MinValue);
        return noiseValue * settings.Strength;
    }
}
