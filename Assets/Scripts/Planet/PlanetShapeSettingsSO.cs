using UnityEngine;

[CreateAssetMenu(fileName = "New Planet Shape Settings", menuName = "Planet Settings/Planet Shape Settings")]
public class PlanetShapeSettingsSO : ScriptableObject
{
    public float PlanetRadius = 1f;
    public PlanetNoiseSettings NoiseSettings;
}
