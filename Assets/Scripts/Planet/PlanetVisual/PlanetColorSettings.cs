using UnityEngine;

//[CreateAssetMenu(fileName = "New Planet Color Settings", menuName = "Planet Settings/Planet Color Settings")]

[System.Serializable]
public class PlanetColorSettings 
{
    public Gradient Gradient;
    public Material Material;

    public PlanetColorSettings(Gradient gradient, Material material)
    {
        Gradient = gradient;
        Material = material;
    }
}
