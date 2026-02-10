using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string factionName { get; private set; }
    public float money { get; private set; }
    public Dictionary<ShipType, int> shipCounts = new Dictionary<ShipType, int>();

    public Faction(FactionDataSO factionData)
    {
        factionName = factionData.factionName;
        money = factionData.money;
        foreach (var shipData in factionData.startingShipDatas)
        {
            shipCounts[shipData.shipType] = shipData.amount;
        }
    }

    public virtual void StartWorldTurn()
    {
        Debug.Log($"{factionName} is doing stuff");
        EndWorldTurn();
    }

    public virtual void EndWorldTurn()
    {
        Debug.Log($"{factionName} has finished doing stuff");
    }
}
