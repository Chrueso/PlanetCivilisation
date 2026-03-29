using System.Collections.Generic;
using UnityEngine;

public class AIContext 
{
    public AIBrain Brain { get; private set; }
    public EntityController Controller { get; private set; }

    public EntityModel Model => Controller.GetModel();
    public int AttackPower => Model.CalculateAttackPower();

    public MapGrid MapGrid => Controller.MapGrid;
    public HashSet<GridHex> HexesInMoveRadius => Controller.HexesInMoveRadius;
    public GridHex CurrentHex => Model.CurrentHex;
    public HashSet<GridHex> VisitedHexes = new HashSet<GridHex>();
    public GridHex LastVisitedHex;

    // Planet state
    public PlanetData CurrentPlanet => CurrentHex.Occupant as PlanetData; //doing it this way makes it so i never have to null check in the actions
    public bool IsOnPlanet => CurrentPlanet != null;
    public bool IsOnUninhabitedPlanet => IsOnPlanet && CurrentPlanet.FactionType == FactionType.Nothing;
    public bool IsOnOwnedPlanet => IsOnPlanet && CurrentPlanet.FactionType == Model.FactionType;
    public bool IsOnEnemyPlanet => IsOnPlanet && !IsOnUninhabitedPlanet && !IsOnOwnedPlanet;

    public AIContext(AIBrain brain, EntityController controller)
    {
        Brain = brain;
        this.Controller = controller;
        VisitedHexes.Add(CurrentHex);
        LastVisitedHex = CurrentHex;
    }
}
