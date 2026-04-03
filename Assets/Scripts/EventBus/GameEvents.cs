using UnityEngine;
using System.Collections.Generic;

//Use structs cause more memory efficient

public struct GameStartEvent : IEvent
{
    public MapGrid MapGrid;
    public IEntityController PlayerController;
    public HashSet<IEntityController> AIControllers;
}

public struct TurnChangeEvent : IEvent
{
    public int CurrentTurn;
    public FactionType PrevTurnFaction;
    public FactionType CurrentTurnFaction;
    public IEntityController CurrentEntity;
}

public struct HUDEntityChangeEvent : IEvent
{
    public IEntityController NewEntity;
}
