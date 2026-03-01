using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    private int width = 45;
    private int height = 27;
    private float cellSize = 5f;

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

        float colWidth = grid.TotalWidth + grid.CellSize * 0.5f;
        float colHeight = (grid.TotalHeight * 0.75f) + (grid.CellSize * 0.25f);
        col.size = new Vector3(colWidth, 0f, colHeight);

        float colCenterX = (grid.TotalWidth * 0.5f) - (grid.CellSize * 0.25f);
        float colCenterZ = (colHeight * 0.5f) - (grid.CellSize * 0.5f);

        col.center = new Vector3(colCenterX, 0f, colCenterZ) + grid.OriginPosition;

        col.isTrigger = true;


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridHex hex = grid.GetGridObject(x, z);
                GameObject obj = Instantiate(hexPrefab, grid.GetWorldPosition(x, z), Quaternion.identity, this.transform);
                obj.name = ("Hex " + x + ", " + z);

                GridHexVisual hexVisual = obj.GetComponent<GridHexVisual>();
                hexVisual.Init(hex);
                hex.GridHexVisual = hexVisual;
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
            GridHex hex = grid.GetGridObject(hit.point);

            if (hex == null) return;

            Debug.Log(hex.WorldPosition);
            Debug.Log(hex.GridPosition);
            hex.GridHexVisual.OnSelected();

        }
    }
}
