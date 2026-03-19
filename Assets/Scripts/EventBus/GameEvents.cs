using UnityEngine;
using System.Collections.Generic;

public struct GameStartEvent : IEvent
{
    public CommandInvoker CommandInvoker;

    public MapGrid MapGrid;

    public PlayerController PlayerController;
    public EntityModel PlayerModel;

    public List<EntityModel> AIEntities;
}

public struct TurnChangeEvent : IEvent
{
    public int CurrentTurn;
    public FactionType PrevTurnFaction;
    public FactionType CurrentTurnFaction;
}
