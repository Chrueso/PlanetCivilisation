using UnityEngine;

public class PlanetColorGenerator 
{
    private PlanetColorSettings settings;
    private Texture2D texture;
    private const int textureResolution = 50;
    private MaterialPropertyBlock propertyBlock;

    public void UpdateSettings(PlanetColorSettings settings)
    {
        this.settings = settings;

        if (texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }

        if (propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void UpdateColors(Renderer[] renderers, MinMaxf elevationMinMax)
    {
        Color[] colors = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = settings.Gradient.Evaluate(i / (textureResolution - 1f)); // This returns the color at position t 0 is first color 0.5 is mid color 1 is last
        }

        texture.SetPixels(colors);
        texture.Apply();

        propertyBlock.Clear();
        propertyBlock.SetTexture("_planetTexture", texture);
        propertyBlock.SetVector("_elevationMinMax",
            new Vector4(elevationMinMax.Min, elevationMinMax.Max));

        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.SetPropertyBlock(propertyBlock);
            }
        }
    }

    public void Cleanup()
    {
        if (texture != null)
        {
            if (Application.isPlaying)
                Object.Destroy(texture);
            else
                Object.DestroyImmediate(texture); //for editor
        }
    }
}
