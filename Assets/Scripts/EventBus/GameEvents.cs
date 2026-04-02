using UnityEngine;
using System.Collections.Generic;

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
