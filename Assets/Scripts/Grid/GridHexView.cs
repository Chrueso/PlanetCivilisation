using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class GridHexView : MonoBehaviour
{
    private float cellSize = 1;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock propertyBlock;

    [SerializeField] private Material material;
    [SerializeField] private Material fogMaterial;
    [SerializeField] private float outlineThickness = 0.1f;
    [SerializeField] private Color outlineColor = Color.cyan;
    [SerializeField] private Color hexColor = Color.black;

    public float OutlineThickness => outlineThickness;

    private Dictionary<string, bool> edgeBoolValues = new Dictionary<string, bool>()
    {
        { "_Edge0", true }, // left
        { "_Edge1", true }, // top left
        { "_Edge2", true }, // top right
        { "_Edge3", true }, // right
        { "_Edge4", true }, // bottom right
        { "_Edge5", true } // bottom left
    };

    public void Init(float cellSize)
    {
        this.cellSize = cellSize;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();    
        meshFilter.mesh = new Mesh();
        propertyBlock = new MaterialPropertyBlock();

        GenerateMesh();
        UpdateMaterial();
    }

    public void GenerateMesh()
    {
        if (cellSize < 0)
        {
            Debug.Log("Grid hex view cannot generate cellsize less than 0");
            return;
        }
        Mesh mesh = meshFilter.mesh;

        // Create vertices
        Vector3[] vertices = new Vector3[7];
        float scaleX = 2f / Mathf.Sqrt(3f);
        float radius = cellSize * 0.5f;

        vertices[0] = Vector3.zero;
        for (int i = 0; i < 6; i++)
        {
            float angle = (90f + 60f * i) * Mathf.Deg2Rad; //+90 start at 90deg cause tip needs to be up
            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );
        }

        Vector2[] uvs = new Vector2[7]; 

        uvs[0] = new Vector2(0.5f, 0.5f);

        // corner UVs
        for (int i = 0; i < 6; i++)
        {
            float angle = (90f + 60f * i) * Mathf.Deg2Rad;
            uvs[i + 1] = new Vector2(
                (Mathf.Cos(angle) * 0.5f) + 0.5f, // addingmap -1..1 circle to 0..1 texture
                (Mathf.Sin(angle) * 0.5f) + 0.5f
            );
        }

        int[] triangles = new int[6 * 3]; // 6 triangles, 3 indices each
        for (int i = 0; i < 6; i++)
        {
            triangles[i * 3] = 0; // center vertex
            triangles[i * 3 + 1] = i + 1 == 6 ? 1 : i + 2; // next corner
            triangles[i * 3 + 2] = i + 1; // current corner
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void UpdateMaterial() 
    {
        if (material != null) meshRenderer.material = material;

        meshRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_Radius", cellSize * 0.5f);
        propertyBlock.SetFloat("_Thickness", outlineThickness);
        propertyBlock.SetColor("_OutlineColor", outlineColor);
        propertyBlock.SetColor("_HexColor", hexColor);

        EnableEdges(propertyBlock, true);

        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void EnableEdges(MaterialPropertyBlock propertyBlock, bool value)
    {
        foreach (var key in edgeBoolValues.Keys.ToList()) // ToList() snapshots the keys
        {
            edgeBoolValues[key] = value;
            propertyBlock.SetFloat(key, value ? 1f : 0f);
        }
    }

    public void ShowFog()
    {
        if (fogMaterial != null) meshRenderer.material = fogMaterial;
    }

    public void HideFog()
    {
        UpdateMaterial();
        Debug.Log("as");
    }

}
