using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingShipData
{
    public ShipType shipType;
    public int amount;
}

[CreateAssetMenu(fileName = "New Faction Data", menuName = "Faction/Faction Data")]
public class FactionDataSO : ScriptableObject
{
    public string factionName;
    public float money;

    public List<StartingShipData> startingShipDatas = new List<StartingShipData>();
    public Dictionary<ShipType, int> shipCounts = new Dictionary<ShipType, int>();
}
