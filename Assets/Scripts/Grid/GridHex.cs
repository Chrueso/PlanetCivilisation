using UnityEngine;

public enum GridHexNeighbourDir
{
    LEFT, TOP_LEFT, TOP_RIGHT, RIGHT, BOTTOM_RIGHT, BOTTOM_LEFT
}

public class GridHex : IHideable
{
    public float CellSize { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    public Vector3Int GridPositionCube { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public bool IsOccupied;
    public IGridHexObject Occupant;
    public GridHexView View;
    public bool IsHiddenForPlayer = false;

    public GridHex(float cellSize, Vector2Int gridPosition, Vector3Int gridPositionCube, Vector3 worldPosition, bool isOccupied = false, IGridHexObject occupant = null)
    {
        CellSize = cellSize;
        GridPosition = gridPosition;
        GridPositionCube = gridPositionCube;
        WorldPosition = worldPosition;
        IsOccupied = isOccupied;
        Occupant = occupant;
    }

    public void Show()
    {
        IsHiddenForPlayer = false;
        View.HideFog();

        if (Occupant != null) Occupant.Show();
    }

    public void Hide()
    {
        IsHiddenForPlayer = true;
        View.ShowFog();
        if (Occupant != null) Occupant.Hide();
    }
}
