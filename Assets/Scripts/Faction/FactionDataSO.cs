using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingShipData
{
    public ShipType ShipType;
    public int Amount;
}

public enum FactionType
{
    Nothing,
    Human,
    DemiHuman,
    IntelligentConstruct,
    Adapters
}

[CreateAssetMenu(fileName = "New Faction Data", menuName = "Faction/Faction Data")]
public class FactionDataSO : ScriptableObject
{
    public FactionType FactionType;
    public float Money;

    public List<StartingShipData> startingShipDatas = new List<StartingShipData>();
    public Dictionary<ShipType, int> shipCounts = new Dictionary<ShipType, int>();
}
