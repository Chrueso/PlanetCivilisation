using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    private GridHex hex;

    public float OutlineThickness => defaultoutlineThickness;

    private Dictionary<GridHexDir, string> edgeBools = new Dictionary<GridHexDir, string>()
    {
        { GridHexDir.RIGHT,         "_Edge0" },
        { GridHexDir.TOP_RIGHT,     "_Edge1" },
        { GridHexDir.TOP_LEFT,    "_Edge2" },
        { GridHexDir.LEFT,        "_Edge3" },
        { GridHexDir.BOTTOM_LEFT, "_Edge4" },
        { GridHexDir.BOTTOM_RIGHT,  "_Edge5" },
    };

    private Dictionary<GridHexDir, string> edgeColors = new Dictionary<GridHexDir, string>()
    {
        { GridHexDir.RIGHT,         "_Edge0Color" },
        { GridHexDir.TOP_RIGHT,     "_Edge1Color" },
        { GridHexDir.TOP_LEFT,    "_Edge2Color" },
        { GridHexDir.LEFT,        "_Edge3Color" },
        { GridHexDir.BOTTOM_LEFT, "_Edge4Color" },
        { GridHexDir.BOTTOM_RIGHT,  "_Edge5Color" },
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
        //propertyBlock.SetColor("_OutlineColor", outlineColor);
        propertyBlock.SetColor("_HexColor", hexColor);
        SetEdgeColors(outlineColor);
        if (hex != null)
        {
            if (hex.OccupyingFaction != FactionType.Nothing)
            {
                hex.ShowHighlight();
            }
        }
        
        EnableEdges(true);

        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void RestoreDefaultMaterial()
    {
        if (hex!= null)
        {
            if (hex.OccupyingFaction != FactionType.Nothing)
            {
                return;
            }
        }
        outlineThickness = defaultoutlineThickness;
        outlineColor = defaultoutlineColor;
        hexColor = defaulthexColor;

        UpdateMaterial();
    }

    public void SetEdgeColors(Color color)
    {
        Debug.Log($"Hex: {hex}, Color {color}, Thingy {color == Color.blue}");
        foreach (var key in edgeColors.Keys.ToList()) // ToList() snapshots the keys
        {
            propertyBlock.SetColor(edgeColors[key], color);
        }
        propertyBlock.SetColor("_OutlineColor", color);
        meshRenderer.SetPropertyBlock(propertyBlock);

    }

    public void SetEdgeColor(GridHexDir dir, Color color)
    {
        propertyBlock.SetColor(edgeColors[dir], color);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void EnableEdges(bool value)
    {
        foreach (var key in edgeBools.Keys.ToList()) // ToList() snapshots the keys
        {
            propertyBlock.SetFloat(edgeBools[key], value ? 1f : 0f);
        }
    }

    public void ShowFog()
    {
        if (fogMaterial != null) meshRenderer.material = fogMaterial;
    }

    public void HideFog(GridHex hex)
    {
        this.hex = hex; 
        UpdateMaterial();
    }

    //public void ChangeOutlineColor(Color color)
    //{
    //    outlineColor = color;
    //    propertyBlock.SetColor("_OutlineColor", outlineColor);
    //    meshRenderer.SetPropertyBlock(propertyBlock);
    //}

    public void HideEdge(GridHexDir dir)
    {
        //propertyBlock.SetFloat(edgeBools[dir], 0f);
        //meshRenderer.SetPropertyBlock(propertyBlock);
        if (hex != null)
        {
            if (hex.OccupyingFaction != FactionType.Nothing)
            {
                return;
            }
        }
        propertyBlock.SetColor(edgeColors[dir], defaultoutlineColor);
        meshRenderer.SetPropertyBlock(propertyBlock);

    }

}
