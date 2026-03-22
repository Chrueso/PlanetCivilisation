using UnityEngine;
using System.Collections.Generic;

public struct GameStartEvent : IEvent
{
    public CommandInvoker CommandInvoker;
    public TurnManager TurnManager;

    public MapGrid MapGrid;

    public EntityController PlayerController;
    public EntityData PlayerModel;

    public List<EntityData> AIEntities;
}

public struct TurnChangeEvent : IEvent
{
    public int CurrentTurn;
    public FactionType PrevTurnFaction;
    public FactionType CurrentTurnFaction;
}
