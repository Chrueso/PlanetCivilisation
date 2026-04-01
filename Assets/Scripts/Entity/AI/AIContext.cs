using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public GridHex LastHex;
    public Dictionary<GridHex, float> VisitedHexes = new Dictionary<GridHex, float>(); 

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
        LastHex = CurrentHex;
        VisitedHexes.Add(CurrentHex, 1f);
    }

    public void AddLastVisitedHex(GridHex hex)
    {
        if (VisitedHexes.ContainsKey(hex))
        {
            VisitedHexes[hex] = 1f; // Reset recency if hex is visited again
        }
        else
        {
            VisitedHexes.Add(hex, 1f);
        }
    }

    public void UpdateVisitedHexesRecency()
    {
        var toRemove = new List<GridHex>();

        foreach (var hex in VisitedHexes.Keys.ToList()) 
        {
            VisitedHexes[hex] -= 0.2f;

            if (VisitedHexes[hex] <= 0f)
                toRemove.Add(hex);
        }

        foreach (var hex in toRemove)
            VisitedHexes.Remove(hex);
    }
}
