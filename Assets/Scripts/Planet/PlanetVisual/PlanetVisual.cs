using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    [Range(2, 256)] // 256^2 is max amount vertices a mesh can have in unity
    public int Resolution = 30;

    public bool AutoUpdate = false;

    public PlanetShapeSettings ShapeSettings;
    public PlanetColorSettings ColorSettings;

    // For inspector
    [HideInInspector]
    public bool ShapeSettingsFoldout;   
    public bool ColorSettingsFoldout;

    private PlanetShapeGenerator shapeGenerator = new PlanetShapeGenerator();
    private PlanetColorGenerator colorGenerator = new PlanetColorGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    // Store renderers for property blocks
    private Renderer[] renderers;

    // Each planet needs its own texture
    private Texture2D planetTexture;

    // Property block for per-planet properties
    private MaterialPropertyBlock propertyBlock;

    private void Init(PlanetShapeSettings shapeSettings, PlanetColorSettings colorSettings)
    {
        ShapeSettings = shapeSettings;
        ColorSettings = colorSettings;

        UpdatePlanetVisual();
    }

    public void UpdatePlanetVisual()
    {
        shapeGenerator.UpdateSettings(ShapeSettings);
        colorGenerator.UpdateSettings(ColorSettings);

        // To create the planet sphere we create 6 faces of a cube and then project them onto a sphere
        // Refer to TerrainFace.cs as to how the mesh is generated for each face

        // Only initialize mesh filters if they are null
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }

        terrainFaces = new TerrainFace[6];
        renderers = new Renderer[6];

        Vector3[] directions = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null) // Only create new mesh filter if it doesnt already exist
            {
                GameObject meshObj = new GameObject("Mesh");
                meshObj.transform.SetParent(this.transform, false);

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshObj.hideFlags = HideFlags.NotEditable;
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = ColorSettings.Material;
            renderers[i] = meshFilters[i].GetComponent<MeshRenderer>();
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, Resolution, directions[i]);

            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
        }
    }

    public void GeneratePlanetVisual(PlanetShapeSettings shapeSettings, PlanetColorSettings colorSettings)
    {
        Init(shapeSettings, colorSettings);
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!AutoUpdate) return;
        UpdatePlanetVisual();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        if (!AutoUpdate) return;
        UpdatePlanetVisual();
        GenerateColors();
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }

        colorGenerator.UpdateElevation(shapeGenerator.ElevationMinMax);
    }

    private void GenerateColors()
    {
        if (renderers == null || renderers.Length == 0) return;

        // Create texture if needed
        if (planetTexture == null)
        {
            planetTexture = new Texture2D(50, 1);
        }

        // Fill texture with gradient colors
        Color[] colors = new Color[50];
        for (int i = 0; i < 50; i++)
        {
            float t = i / 49f;
            colors[i] = ColorSettings.Gradient.Evaluate(t);
        }
        planetTexture.SetPixels(colors);
        planetTexture.Apply();

        // Set per-planet properties using property block
        propertyBlock.Clear();
        propertyBlock.SetTexture("_planetTexture", planetTexture);
        propertyBlock.SetVector("_elevationMinMax",
            new Vector4(shapeGenerator.ElevationMinMax.Min,
                       shapeGenerator.ElevationMinMax.Max));

        // Apply to all 6 faces of THIS planet only
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        Debug.Log($"Updated colors for {gameObject.name} with texture ID: {planetTexture.GetInstanceID()}");
    }

    // Clean up texture when planet is destroyed
    private void OnDestroy()
    {
        if (planetTexture != null)
        {
            if (Application.isPlaying)
                Destroy(planetTexture);
            else
                DestroyImmediate(planetTexture);
        }
    }
}

    //private void GenerateColors()
    //{
    //   colorGenerator.UpdateColors();
    //}

