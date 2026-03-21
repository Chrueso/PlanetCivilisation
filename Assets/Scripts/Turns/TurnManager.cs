using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    private int currentTurn = 0;
    private FactionType currentTurnFaction = FactionType.Nothing;
    private int turnIndex = -1; 
    private List<FactionType> turnOrder = new List<FactionType>();

    private EventBinding<GameStartEvent> gameStartBinding;

    public TurnManager()
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        FactionType playerFaction = gameStartEvent.PlayerModel.FactionType;
        List<FactionType> AIfactions = new List<FactionType>();
        foreach (var ai in gameStartEvent.AIEntities)
        {
            AIfactions.Add(ai.FactionType);
        }

        SetupTurnOrder(playerFaction, AIfactions);
        ChangeTurn();
    }

    private void SetupTurnOrder(FactionType playerFaction, List<FactionType> AIfactions)
    {
        turnOrder.Clear();

        if (!turnOrder.Contains(playerFaction))
        {
            turnOrder.Add(playerFaction);
        }
        else
        {
            Debug.LogError("Duplicate player faction in turn order!");
            return;
        }

        foreach (var faction in AIfactions)
        {
            if (!turnOrder.Contains(faction))
            {
                turnOrder.Add(faction);
            }
            else
            {
                Debug.LogError("Duplicate AI faction in turn order!");
            }
        }
    }

    public void ChangeTurn()
    {
        currentTurn++;
        turnIndex = (turnIndex + 1) % turnOrder.Count; // increment index but loops around 
        var prevTurnFaction = currentTurnFaction;
        currentTurnFaction = turnOrder[turnIndex];

        Debug.Log($"Current turn: [{currentTurnFaction}]");

        EventBus<TurnChangeEvent>.Raise(new TurnChangeEvent
        {
            CurrentTurn = currentTurn,
            PrevTurnFaction = prevTurnFaction,
            CurrentTurnFaction = currentTurnFaction
        });
    }
}
