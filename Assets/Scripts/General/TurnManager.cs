using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.DefaultInputActions;

public class TurnManager
{
    private GameConfigSO gameConfig;
    private int currentTurn = 0;
    private FactionType currentTurnFaction = FactionType.Nothing;
    private int turnIndex = -1; 
    private List<FactionType> turnOrder = new List<FactionType>();
    // chris - i need to have the peeps
    private IEntityController currEntityController = null;
    private List<IEntityController> entityControllerOrder = new List<IEntityController>();


    private EventBinding<GameStartEvent> gameStartBinding;

    public TurnManager(GameConfigSO gameConfig)
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
        this.gameConfig = gameConfig;
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        FactionType playerFaction = gameStartEvent.PlayerController.GetModel().FactionType;
        List<FactionType> AIfactions = new List<FactionType>();
        foreach (var ai in gameStartEvent.AIControllers)
        {
            AIfactions.Add(ai.GetModel().FactionType);
        }
        SetupEntityOrder(gameStartEvent.PlayerController, gameStartEvent.AIControllers.ToList());
        SetupTurnOrder(playerFaction, AIfactions);
    }
    // added by chris
    private void SetupEntityOrder(IEntityController player, List<IEntityController> AIcontroller)
    {
        entityControllerOrder.Clear();
        if (!entityControllerOrder.Contains(player))
        {
            entityControllerOrder.Add(player);
        } else
        {
            Debug.LogError("Duplicate player faction in entity controller order!");
            return;
        }

        foreach (var faction in AIcontroller)
        {
            if (!entityControllerOrder.Contains(faction))
            {
                entityControllerOrder.Add(faction);
            }
            else
            {
                Debug.LogError("Duplicate AI faction in entity controller order!");
            }
        }
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
        if (currentTurn >= gameConfig.MaxTurns)
        {
            SceneManager.LoadScene("MainMenu");
            IEntityController winner = null;
            int binner = 0;
            foreach (var thingy in entityControllerOrder)
            {
                int ownedPlanets = thingy.GetModel().OwnedPlanets.Count;
                if (ownedPlanets > binner)
                {
                    binner = ownedPlanets;
                    winner = thingy;
                }
            }
            return;
        }
        turnIndex = (turnIndex + 1) % turnOrder.Count; // increment index but loops around 
        var prevTurnFaction = currentTurnFaction;
        currentTurnFaction = turnOrder[turnIndex];
        currEntityController = entityControllerOrder[turnIndex];

        Debug.Log($"Current turn: {currentTurnFaction}");

        EventBus<TurnChangeEvent>.Raise(new TurnChangeEvent
        {
            CurrentTurn = currentTurn,
            PrevTurnFaction = prevTurnFaction,
            CurrentTurnFaction = currentTurnFaction,
            CurrentEntity = currEntityController
        });
    }
}
