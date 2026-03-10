using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Scriptable Objects/MapSettings")]
public class MapSettings : ScriptableObject
{
    public Vector2Int MapDimensions;
    public float MapCellSize;
    public MapGrid MapGridPrefab;

    public int RadiusBetweenPlanets = 2;
    public int MaxPlanets = 20;
    public CustomPlanetSO HomePlanetData;
}
