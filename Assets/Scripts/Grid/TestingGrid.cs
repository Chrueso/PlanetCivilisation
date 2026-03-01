using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    private int width = 3;
    private int height = 3;
    private float cellSize = 10f;

    private Camera cameraInstance;
    private float fingerRayMaxDistance = 1000f;
    HexGridXZ<GridHex> grid;
    BoxCollider col;

    [SerializeField] GameObject hexPrefab;

    private void Start()
    {
        cameraInstance = Camera.main;
        TouchscreenHandler.Instance.FingerDownCallback += OnTouch;

        grid = new HexGridXZ<GridHex>(
            width,
            height,
            cellSize,
            transform.position,
            (x, z) =>
            {
                Vector2Int gridPos = new Vector2Int(x, z);
                Vector3 worldPos = HexGridXZ<GridHex>.GetWorldPosition(x, z, cellSize, transform.position);
                return new GridHex(cellSize, gridPos, worldPos);
            }
        );

        col = GetComponent<BoxCollider>();
        col.size = new Vector3(grid.TotalWidth, 0f, grid.TotalHeight);
        col.center = new Vector3(grid.TotalWidth, 0f, grid.TotalHeight) * 0.5f + grid.OriginPosition;
        col.isTrigger = true;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject obj = Instantiate(hexPrefab, grid.GetWorldPosition(x, z), Quaternion.identity, this.transform);
                obj.name = ("Hex " + x + ", " + z);

                GridHexVisual hexVisual = obj.GetComponent<GridHexVisual>();
                hexVisual.Init(grid.GetGridObject(x, z));
            }
        }
    }

    void OnTouch(object sender, TouchInfo touchInfo)
    {
        if (touchInfo.Index != 0) return;
        Ray fingerRay = cameraInstance.ScreenPointToRay(touchInfo.ScreenPos);
        RaycastHit hit;

        if (Physics.Raycast(fingerRay, out hit, fingerRayMaxDistance))
        {
            Debug.Log(hit.point);
            Vector3Int gridPos = grid.GetPositionOnGrid(hit.point);
            Debug.Log(gridPos);
        }
    }
}
