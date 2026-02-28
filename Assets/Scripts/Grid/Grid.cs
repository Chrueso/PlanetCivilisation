using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Grid 
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    public float TotalWidth { get; private set; }
    public float TotalHeight { get; private set; }

    private int[,] gridArray;
    private TextMeshPro[,] debugTextArray;

    public Grid(int width, int height, float cellSize)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;

        TotalWidth = width * cellSize;
        TotalHeight = height * cellSize;
        
        gridArray = new int[width, height];
        debugTextArray = new TextMeshPro[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                debugTextArray[x,y] = DebugUtil.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPositionCenter(x, y), 20, Color.white, TextAlignmentOptions.Center);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                SetValue(x, y, x + y * width);
            }
        }

        SetValue(0, 0, 67);
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize;      
    }

    public Vector3 GetWorldPositionCenter(int x, int y)
    {
        return (new Vector3(x, y) * CellSize) + (new Vector3(CellSize, CellSize) * 0.5f);
    }

    public Vector2Int GetPositionOnGrid(Vector3 worldPos)
    {
        Vector2Int gridPos = Vector2Int.zero;

        gridPos.x = Mathf.FloorToInt(worldPos.x / CellSize);
        gridPos.y = Mathf.FloorToInt(worldPos.y / CellSize);

        return gridPos;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }
}
