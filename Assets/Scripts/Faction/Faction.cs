using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public FactionType FactionType { get; private set; }
    public float Money { get; private set; }
    public Dictionary<ShipTypeSO, int> shipCounts = new Dictionary<ShipTypeSO, int>();
    public List<PlanetData> ownedPlanets = new List<PlanetData>();

    //added by ysaac
    public bool IsPlayer { get; private set; }
    public int ActionPoints { get; private set; }
    private int baseActionPoints;
    private int maxActionPoints;

    public event System.Action OnTurnFinished;

    public Faction(FactionDataSO factionData)
    {
        FactionType = factionData.FactionType;
        Money = factionData.Money;
        IsPlayer = factionData.IsPlayer;

        baseActionPoints = factionData.BaseActionPoints;
        maxActionPoints = baseActionPoints;
        ActionPoints = maxActionPoints;

        foreach (var shipData in factionData.startingShipDatas)
        {
            shipCounts[shipData.ShipType] = shipData.Amount;
        }
    }

    public virtual void StartPlayerTurn()
    {
        Debug.Log($"{FactionType} is beginning");
        //ActionPoints = maxActionPoints;
        // Enable player controls here
        RunAiTurn();
    }

    public virtual void StartAITurn()
    {
        Debug.Log($"{FactionType} is beginning");
        //ActionPoints = maxActionPoints;
        RunAiTurn();
    }

    public virtual void RunAiTurn()
    {
        while(ActionPoints > 0)
        {
            //do ai stuff here
            ActionPoints--;
            Debug.Log($"{FactionType} did an action");
        }
        TurnManager.Instance.EndTurn();
    }

    public virtual void EndTurn()
    {
        Debug.Log($"{FactionType} has finished doing stuff");
        OnTurnFinished?.Invoke();
    }
}
