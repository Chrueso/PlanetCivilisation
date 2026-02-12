using UnityEngine;

public class NoiseFilter 
{
    private SimplexNoise noise = new SimplexNoise();

    public float Evaluate(Vector3 point)
    {
        float noiseValue = (noise.Evaluate(point) + 1) * 0.5f; // change range from -1-1 to 0-1
        return noiseValue;
    }   
}
