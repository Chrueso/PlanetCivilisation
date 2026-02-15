using UnityEngine;

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
