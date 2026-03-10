using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public HexGridXZ<GridHex> Grid { get; private set; }
    public BoxCollider Col { get; private set; }

    [SerializeField] private GameObject hexPrefab;

    public void GenerateGrid(int width, int height, float cellSize)
    {
        Grid = new HexGridXZ<GridHex>(
            width,
            height,
            cellSize,
            transform.position,
            (x, z) =>
            {
                Vector2Int gridPos = new Vector2Int(x, z);
                Vector3Int gridPosCube = HexGridXZ<GridHex>.OddRToCube(x, z);
                Vector3 worldPos = HexGridXZ<GridHex>.GetWorldPosition(x, z, cellSize, transform.position);
                return new GridHex(cellSize, gridPos, gridPosCube, worldPos);
            }

        );

        Col = GetComponent<BoxCollider>();

        Col.size = new Vector3(Grid.TotalWorldWidth, 0f, Grid.TotalWorldHeight);

        float colCenterX = (Grid.TotalWorldWidth * 0.5f) - Grid.HexRadius;
        float colCenterZ = (Grid.TotalWorldHeight * 0.5f) - Grid.HexRadius;

        Col.center = new Vector3(colCenterX, 0f, colCenterZ) + Grid.OriginPosition;

        Col.isTrigger = true;


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridHex hex = Grid.GetGridObject(x, z);
                GameObject obj = Instantiate(hexPrefab, Grid.GetWorldPosition(x, z), Quaternion.identity, this.transform);
                obj.name = ("Hex " + x + ", " + z);

                GridHexVisual hexVisual = obj.GetComponent<GridHexVisual>();
                hexVisual.Init(hex);
                hex.GridHexVisual = hexVisual;
            }
        }
    }


}
