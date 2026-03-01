
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
        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = (90f + 60f * i) * Mathf.Deg2Rad; //+90 start at 90deg cause tip needs to be up
            vertices[i] = new Vector3(
                Mathf.Cos(angle) * gridHex.CellSize,
                0,
                Mathf.Sin(angle) * gridHex.CellSize
            );
        }

        // Making hex with 2 parallelogram
        int[] triangles = new int[12]
        {
            // First parallelogram -
            0, 5, 1,  
            1, 5, 2,  

            // Second parallelogram 
            5, 4, 2,  
            2, 4, 3   
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }

    public void UpdateMaterial() 
    {
        meshRenderer.GetPropertyBlock(propertyBlock);   
        propertyBlock.SetColor("_Color", Color.cyan);        
        meshRenderer.SetPropertyBlock(propertyBlock);   
    }





    //public void GenerateMesh(Transform parent)
    //{
    //    InitMeshComponents(parent);
    //    Mesh mesh = meshFilter.mesh;

    //    int width = grid.Width;
    //    int height = grid.Height;
    //    float cellSize = grid.CellSize;

    //    Vector3[] vertices = new Vector3[width * height * 6];
    //    int[] triangles = new int[width * height * 12];

    //    int hexIndex = 0;
    //    for (int z = 0; z < height; z++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            Vector3 hexCenter = grid.GetWorldPositionCenter(x, z);

    //            int vertexIndex = hexIndex * 6;
    //            int triIndex = hexIndex * 12; 

    //            // Add 6 vertices
    //            for (int i = 0; i < 6; i++)
    //            {
    //                float angle = 60f * i * Mathf.Deg2Rad;
    //                vertices[vertexIndex + i] = hexCenter + new Vector3(
    //                    Mathf.Cos(angle) * cellSize,
    //                    0,
    //                    Mathf.Sin(angle) * cellSize
    //                );
    //            }

    //            // Making hex using 2 parallelograms
    //            // First triangle
    //            triangles[triIndex + 0] = vertexIndex + 0;
    //            triangles[triIndex + 1] = vertexIndex + 1;
    //            triangles[triIndex + 2] = vertexIndex + 5;

    //            triangles[triIndex + 3] = vertexIndex + 1;
    //            triangles[triIndex + 4] = vertexIndex + 2;
    //            triangles[triIndex + 5] = vertexIndex + 5;

    //            // Seconnd triangle
    //            triangles[triIndex + 6] = vertexIndex + 5;
    //            triangles[triIndex + 7] = vertexIndex + 2;
    //            triangles[triIndex + 8] = vertexIndex + 4;

    //            triangles[triIndex + 9] = vertexIndex + 2;
    //            triangles[triIndex + 10] = vertexIndex + 3;
    //            triangles[triIndex + 11] = vertexIndex + 4;

    //            hexIndex++;
    //        }
    //        mesh.Clear(); 
    //        mesh.vertices = vertices;
    //        mesh.triangles = triangles;
    //        mesh.RecalculateNormals();
    //    }

    //}


}
