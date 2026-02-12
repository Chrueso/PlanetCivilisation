using UnityEngine;

public class NoiseFilter 
{
    private SimplexNoise noise = new SimplexNoise();
    private PlanetNoiseSettings settings;

    public NoiseFilter(PlanetNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;   
        float frequency = settings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.Layers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.Center);
            noiseValue += (v + 1) * 0.5f * amplitude; // change range from -1-1 to 0-1 and scale by amplitude
            frequency *= settings.Roughness; // if roughness is greateer than 1 frequency increases each layer
            amplitude *= settings.Persistence; // if persistence is less than 1 amplitude decreases each layer
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.MinValue);  
        return noiseValue * settings.Strength;
    }
}
