
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class GridHexVisual : MonoBehaviour
{
    private GridHex gridHex;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
  
    [SerializeField] private Material material;
    private MaterialPropertyBlock propertyBlock;

    private bool isSelected = false;
    [SerializeField] private Color defaultColor = Color.cyan;
    [SerializeField] private Color selectedColor = Color.green;

    public void Init(GridHex gridHex)
    {
        this.gridHex = gridHex;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();    
        meshFilter.mesh = new Mesh();
        propertyBlock = new MaterialPropertyBlock();

        GenerateMesh();
        UpdateMaterial();
    }

    public void GenerateMesh()
    {
        Mesh mesh = meshFilter.mesh;

        // Create vertices
        Vector3[] vertices = new Vector3[7];
        float scaleX = 2f / Mathf.Sqrt(3f);
        float radius = gridHex.CellSize * 0.5f;

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
        Color currentColor = isSelected ? selectedColor : defaultColor;
        propertyBlock.SetColor("_OutlineColor", currentColor);
        meshRenderer.SetPropertyBlock(propertyBlock);   
    }

    public void OnSelected()
    {
        isSelected = !isSelected;
        UpdateMaterial();
    }


}
