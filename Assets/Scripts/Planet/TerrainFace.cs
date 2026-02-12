using UnityEngine;

public class TerrainFace 
{
    private PlanetShapeGenerator shapeGenerator;
    private Mesh mesh;
    private int resolution; // Number of vertices along one edge of the face
    private Vector3 localUp;

    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(PlanetShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA); // Find a perpendicular vector to localUp and axisA
    }

    public void ConstructMesh()
    {
        // Total vertices for one face is resolution squared
        Vector3[] vertices = new Vector3[resolution * resolution];

        // Formula for number of triangles needed
        // (resolution - 1) ^ 2 * 2 * 3 gives total number of triangles for one face

        // (resolution - 1) ^ 2 gives number of quads on the face
        // Each quad consists of 2 triangles so times 2
        // Each triangles need 3 vertices so thats why u multiply 3 
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        // Making the triangles and vertices
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution; // Current vertex index

                // Get percentage across the face
                // When x = 0, percent.x = 0 when x = resolution - 1, percent.x = 1
                // This is basically the step but normalized (0 to resolution, but normalized to 0 to 1)
                // We use this to place the vertices
                Vector2 percent = new Vector2(x, y) / (resolution - 1); 

                // To get point on a face
                // Example if unit cube is at 0,0,0 to get point on top face u move 1 unit at the local up axis
                // We want to start at corner then build the grid of vertices of the face
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2f * axisB;

                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized; // Normalize to project onto sphere (it changes the corners to be rounded)  

                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    // Each square on the grid is made of 2 triangles
                    // Triangle 1
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    // Triangle 2
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6; // Move to next set of triangle indices
                }
            }

            mesh.Clear(); // Do this so when updating to lower resolution it doesnt refereence the old higher res data causing null ref
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }   
    }
}
