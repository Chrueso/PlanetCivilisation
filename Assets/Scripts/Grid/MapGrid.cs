using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class MapGrid : MonoBehaviour
{
    public HexGridXZ<GridHex> Grid { get; private set; }
    public BoxCollider Col { get; private set; }

    [SerializeField] private GridHexView hexViewPrefab;

    public GridHexView HexView => hexViewPrefab;
    public float CellSize {  get; private set; }

    public void LinkNeighbours()
    {
        if (Grid == null || Grid.GridArray.Length == 0) return;

        foreach (GridHex hex in Grid.GridArray)
        {
            hex.SetNeighbours(Grid.GetAllNeighbours(hex.GridPositionCube));
        }
    }
    public void GenerateGrid(int width, int height, float cellSize)
    {
        CellSize = cellSize;

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
                GridHexView hexView = Instantiate(hexViewPrefab, Grid.GetWorldPosition(x, z), Quaternion.identity, this.transform);
                hexView.name = ("Hex " + x + ", " + z);
                hexView.Init(hex.CellSize);
                hex.View = hexView;
                hex.Hide();
            }
        }
    }


}
