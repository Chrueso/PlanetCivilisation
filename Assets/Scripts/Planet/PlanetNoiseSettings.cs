using UnityEngine;

[System.Serializable]
public class PlanetNoiseSettings 
{
    public float Strength = 1f;

    [Range(1f, 8f)]
    public int Layers = 1;

    public float Persistence = 0.5f; // Amplitude change per layer if 0.5 then each layer is half the amplitude of the previous layer

    public float BaseRoughness = 1f;    
    public float Roughness = 2f;

    public Vector3 Center;

    public float MinValue;
}
