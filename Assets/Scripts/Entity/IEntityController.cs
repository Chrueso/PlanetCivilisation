using System.Collections.Generic;
using UnityEngine;

public interface IEntityController
{
    public FactionType GetFaction();

    public GridHex GetCurrentHex();

    public bool CheckIfHexIsInMoveRadius(GridHex hex);

    public void UpdateHexesInMoveRadius();

    public bool CanExecuteAction();

    public bool IsCurrentTurn { get; }

    public bool TryEndTurn();

    public bool TryMove(GridHex hex);

    public bool TryColonize(PlanetData planet);

    public bool TryAttack(PlanetData planet);

    public bool TryBuildStructure(PlanetData planet, StructureType structure);

    
    // Diplomacy
    public void Trade(PlanetData planet);

    public void Gift(PlanetData planet);

    public void Agreement(PlanetData planet);
}
