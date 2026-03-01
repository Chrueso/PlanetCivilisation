using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public FactionType FactionType { get; private set; }
    public float Money { get; private set; }
    public Dictionary<ShipTypeSO, int> shipCounts = new Dictionary<ShipTypeSO, int>();
    public List<PlanetData> ownedPlanets = new List<PlanetData>();

    public Faction(FactionDataSO factionData)
    {
        FactionType = factionData.FactionType;
        Money = factionData.Money;
        foreach (var shipData in factionData.startingShipDatas)
        {
            shipCounts[shipData.ShipType] = shipData.Amount;
        }
    }

    public virtual void StartWorldTurn()
    {
        Debug.Log($"{FactionType} is doing stuff");
        EndWorldTurn();
    }

    public virtual void EndWorldTurn()
    {
        Debug.Log($"{FactionType} has finished doing stuff");
    }
}
