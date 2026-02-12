using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    [Range(2, 256)] // 256^2 is max amount vertices a mesh can have in unity
    public int Resolution = 10;

    public PlanetShapeSettingsSO ShapeSettings;
    public PlanetColorSettingsSO ColorSettings;

    private PlanetShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        GeneratePlanetVisual();
        Init();
        GenerateMesh();
    }

    private void Init()
    {
        shapeGenerator = new PlanetShapeGenerator(ShapeSettings);

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

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = Resources.Load<Material>("Materials/PlanetMaterial");
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

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
        Init();
        GenerateMesh();
    }   

    public void OnColorSettingsUpdated()
    {
        Init();
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
        foreach (MeshFilter meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = ColorSettings.PlanetColor;
        }
    }
}
