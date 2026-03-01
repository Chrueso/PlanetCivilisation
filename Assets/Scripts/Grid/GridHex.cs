using UnityEngine;

public class GridHex 
{
    public float CellSize { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public bool IsOccupied;
    public IGridHexOccupant Occupant;
    public GridHexVisual GridHexVisual;

    public GridHex(float cellSize, Vector2Int gridPosition, Vector3 worldPosition, bool isOccupied = false, IGridHexOccupant occupant = null)
    {
        CellSize = cellSize;
        GridPosition = gridPosition;
        WorldPosition = worldPosition;
        IsOccupied = isOccupied;
        Occupant = occupant;
    }
}
