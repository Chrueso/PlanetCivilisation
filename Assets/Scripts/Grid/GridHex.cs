using System.Collections.Generic;
using UnityEngine;

public class GridHex : IHideable
{
    public float CellSize { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    public Vector3Int GridPositionCube { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public IGridHexObject Occupant;
    public GridHexView View;
    public bool IsHiddenForPlayer = false;
    public FactionType OccupyingFaction = FactionType.Nothing;
    public Dictionary<FactionType, Color> colorCodes = new Dictionary<FactionType, Color>() {
        { FactionType.Human, Color.orange },
        { FactionType.DemiHuman,Color.purple },
        { FactionType.IntelligentConstruct, Color.grey}
    };

    public Dictionary<GridHexDir, GridHex> Neighbours { get; private set; } = new Dictionary<GridHexDir, GridHex>();

    public bool IsHighlighted { get; private set; } = false;
    public Color HighlightColor = Color.white;

    public GridHex() { }
    public GridHex(float cellSize, Vector2Int gridPosition, Vector3Int gridPositionCube, Vector3 worldPosition, bool isOccupied = false, IGridHexObject occupant = null)
    {
        CellSize = cellSize;
        GridPosition = gridPosition;
        GridPositionCube = gridPositionCube;
        WorldPosition = worldPosition;
        Occupant = occupant;
    }

    public void SetNeighbours(Dictionary<GridHexDir, GridHex> neighbours)
    {
        Neighbours.Clear();
        Neighbours = neighbours;
    }

    public void Show()
    {
        IsHiddenForPlayer = false;
        View.HideFog(this);

        if (Occupant != null) Occupant.Show();
    }

    public void Hide()
    {
        IsHiddenForPlayer = true;
        View.ShowFog();
        if (Occupant != null) Occupant.Hide();
    }

    public void ShowHighlight(Color color)
    {
        if (OccupyingFaction != FactionType.Nothing)
        {
            return;
        }
        IsHighlighted = true;
        View.SetEdgeColors(color);
    }

    public void ShowHighlight()
    {
        Debug.Log(OccupyingFaction);
        if (OccupyingFaction == FactionType.Nothing)
        {
            return;
        }
        IsHighlighted = true;
        View.SetEdgeColors(colorCodes[OccupyingFaction]);
    }

    public void SetOccupyingFaction(FactionType factionType)
    {
        OccupyingFaction = factionType;
    }

    public void OffHighlight()
    {
        if (OccupyingFaction == FactionType.Nothing)
        {
            IsHighlighted = false;
            View.RestoreDefaultMaterial();
            return;
        }
        View.SetEdgeColors(colorCodes[OccupyingFaction]);
    }

    public void FixEdges()
    {
        if (Neighbours.Count > 0)
        {
            foreach (var kvp in Neighbours)
            {
                if (kvp.Value == null) continue;

                if (kvp.Value.IsHighlighted)
                {
                    View.HideEdge(kvp.Key);
                }
            }
        }
    }
}
