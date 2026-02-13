using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    [Range(2, 256)] // 256^2 is max amount vertices a mesh can have in unity
    public int Resolution = 10;

    public bool AutoUpdate = true;  

    public PlanetShapeSettingsSO ShapeSettings;
    public PlanetColorSettingsSO ColorSettings;

    // For inspector
    [HideInInspector]
    public bool ShapeSettingsFoldout;   
    public bool ColorSettingsFoldout;

    private PlanetShapeGenerator shapeGenerator = new PlanetShapeGenerator();
    private PlanetColorGenerator colorGenerator = new PlanetColorGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private void Init()
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
                meshObj.transform.parent = this.transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = ColorSettings.Material;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, Resolution, directions[i]);
        }
    }

    public void GeneratePlanetVisual()
    {
        Init();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (!AutoUpdate) return;
        Init();
        GenerateMesh();
    }   

    public void OnColorSettingsUpdated()
    {
        if (!AutoUpdate) return;
        Init();
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
       colorGenerator.UpdateColors();
    }
}
