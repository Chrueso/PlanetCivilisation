using UnityEngine;

public class PlanetColorGenerator 
{
    private PlanetColorSettingsSO settings;
    private Texture2D texture;
    private const int textureResolution = 50;

    public void UpdateSettings(PlanetColorSettingsSO settings)
    {
        this.settings = settings;

        if (texture == null)
        {
            texture = new Texture2D(textureResolution, 1);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.Material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[textureResolution];
        for (int i = 0; i < textureResolution; i++)
        {
            colors[i] = settings.Gradient.Evaluate(i / (textureResolution - 1f)); // This returns the color at position t 0 is first color 0.5 is mid color 1 is last
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.Material.SetTexture("_planetTexture", texture);
    }
}
