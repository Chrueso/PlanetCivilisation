using UnityEngine;

[CreateAssetMenu(fileName = "New Planet Color Settings", menuName = "Planet Settings/Planet Color Settings")]
public class PlanetColorSettingsSO : ScriptableObject
{
    public Gradient Gradient;
    public Material Material;
}
