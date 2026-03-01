using System;
using TMPro;
using UnityEngine;

public class SquareGridXZ <TGridObject>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    public float TotalWidth { get; private set; }
    public float TotalHeight { get; private set; }

    public Vector3 OriginPosition { get; private set; }

    private TGridObject[,] gridArray;

    private TextMeshPro[,] debugTextArray;

    private bool isDebug = true;

    public SquareGridXZ(int width, int height, float cellSize, Vector3 originPos, Func<TGridObject> createGridObj)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;

        TotalWidth = width * cellSize;
        TotalHeight = height * cellSize;

        OriginPosition = originPos;
        
        gridArray = new TGridObject[width, height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                gridArray[x, z] = createGridObj();
            }
        }


        if (isDebug) ShowDebug();
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0f, z) * CellSize + OriginPosition;      
    }

    public Vector3 GetWorldPositionCenter(int x, int z)
    {
        return (new Vector3(x, 0f, z) * CellSize) + (new Vector3(CellSize, 0f, CellSize) * 0.5f) + OriginPosition;
    }

    public Vector3Int GetPositionOnGrid(Vector3 worldPos)
    {
        Vector3Int gridPos = Vector3Int.zero;

        gridPos.x = Mathf.FloorToInt((worldPos - OriginPosition).x / CellSize);
        gridPos.z = Mathf.FloorToInt((worldPos - OriginPosition).z / CellSize);

        return gridPos;
    }

    public TGridObject GetGridObject(Vector3 worldPos)
    {
        Vector3Int gridPos = GetPositionOnGrid(worldPos);

        if (gridPos.x >= 0 && gridPos.z >= 0 &&
        gridPos.x < Width && gridPos.z < Height)
        {
            return gridArray[gridPos.x, gridPos.z];
        }

        return default;
    }

    private void ShowDebug()
    {
        debugTextArray = new TextMeshPro[Width, Height];

        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                debugTextArray[x, z] = DebugUtil.CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPositionCenter(x, z), Quaternion.Euler(90f, 0f, 0f),
                    10, Color.white, TextAlignmentOptions.Center);

                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 100f);
    }

}
