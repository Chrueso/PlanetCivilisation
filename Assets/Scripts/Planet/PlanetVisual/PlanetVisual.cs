using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    [Range(2, 256)] // 256^2 is max amount vertices a mesh can have in unity
    public int Resolution = 30;

    public bool AutoUpdate = false;

    public PlanetShapeSettings ShapeSettings;
    public PlanetColorSettings ColorSettings;

    private PlanetShapeGenerator shapeGenerator = new PlanetShapeGenerator();
    private PlanetColorGenerator colorGenerator = new PlanetColorGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;

    private TerrainFace[] terrainFaces;
    private Renderer[] renderers;

    private void Init(PlanetShapeSettings shapeSettings, PlanetColorSettings colorSettings)
    {
        ShapeSettings = shapeSettings;
        ColorSettings = colorSettings;

        GenerateMeshComponents();
    }

    public void GenerateMeshComponents()
    {
        shapeGenerator.UpdateSettings(ShapeSettings);
        colorGenerator.UpdateSettings(ColorSettings);

        // To create the planet sphere we create 6 faces of a cube and then project them onto a sphere
        // Refer to TerrainFace.cs as to how the mesh is generated for each face

        if (meshFilters == null || meshFilters.Length == 0) meshFilters = new MeshFilter[6];

        terrainFaces = new TerrainFace[6];

        if (renderers == null || renderers.Length == 0) renderers = new Renderer[6];

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
        }
    }

    public void GeneratePlanetVisual(PlanetShapeSettings shapeSettings, PlanetColorSettings colorSettings)
    {
        Init(shapeSettings, colorSettings);
        GenerateMeshComponents();
        GenerateMesh();
        GenerateColors();
    }

    public void UpdatePlanetVisual()
    {
        GenerateMeshComponents();
        GenerateMesh();
        GenerateColors();
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColors()
    {
        colorGenerator.UpdateColors(renderers, shapeGenerator.ElevationMinMax);
    }

    private void OnDestroy()
    {
        colorGenerator.Cleanup();
    }
}

