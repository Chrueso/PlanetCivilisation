using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingShipData
{
    public ShipDataSO ShipType;
    public int Amount;
}

[CreateAssetMenu(fileName = "New Faction Data", menuName = "Faction/Faction Data")]
public class FactionDataSO : ScriptableObject
{
    public FactionType FactionType;
    public float Money;
    public int BaseActionPoints;

    //temp thing to test
    public bool IsPlayer = false;

    public List<StartingShipData> startingShipDatas = new List<StartingShipData>();
    public Dictionary<ShipDataSO, int> shipCounts = new Dictionary<ShipDataSO, int>();
}
