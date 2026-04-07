using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private float defaultoutlineThickness = 0.1f;
    [SerializeField] private Color defaultoutlineColor = Color.cyan;
    [SerializeField] private Color defaulthexColor = Color.black;

    private float outlineThickness;
    private Color outlineColor;
    private Color hexColor;

    public float OutlineThickness => defaultoutlineThickness;

    private Dictionary<NeighbourDir, string> edgeBoolValues = new Dictionary<NeighbourDir, string>()
    {
        { NeighbourDir.RIGHT,         "_Edge0" },
        { NeighbourDir.TOP_RIGHT,     "_Edge1" },
        { NeighbourDir.TOP_LEFT,    "_Edge2" },
        { NeighbourDir.LEFT,        "_Edge3" },
        { NeighbourDir.BOTTOM_LEFT, "_Edge4" },
        { NeighbourDir.BOTTOM_RIGHT,  "_Edge5" },
    };

    public void Init(float cellSize)
    {
        this.cellSize = cellSize;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();    
        meshFilter.mesh = new Mesh();
        propertyBlock = new MaterialPropertyBlock();

        outlineThickness = defaultoutlineThickness;
        outlineColor = defaultoutlineColor;
        hexColor = defaulthexColor;

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

    public void RestoreDefaultMaterial()
    {
        outlineThickness = defaultoutlineThickness;
        outlineColor = defaultoutlineColor;
        hexColor = defaulthexColor;

        UpdateMaterial();
    }

    public void EnableEdges(MaterialPropertyBlock propertyBlock, bool value)
    {
        foreach (var key in edgeBoolValues.Keys.ToList()) // ToList() snapshots the keys
        {
            propertyBlock.SetFloat(edgeBoolValues[key], value ? 1f : 0f);
        }
    }

    public void ShowFog()
    {
        if (fogMaterial != null) meshRenderer.material = fogMaterial;
    }

    public void HideFog()
    {
        UpdateMaterial();
    }

    public void ChangeOutlineColor(Color color)
    {
        outlineColor = color;
        propertyBlock.SetColor("_OutlineColor", outlineColor);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void HideEdge(NeighbourDir dir)
    {
        propertyBlock.SetFloat(edgeBoolValues[dir], 0f);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

}
