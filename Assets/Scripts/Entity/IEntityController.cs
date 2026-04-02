using System.Collections.Generic;
using UnityEngine;

public interface IEntityController
{
    public EntityModel GetModel();

    public EntityView GetView();

    public void HandleCurrrentHexChanged();

    public bool CheckIfHexIsInMoveRadius(GridHex hex);

    public void UpdateHexesInMoveRadius();

    public void UpdateVision();

    public bool CanExecuteAction();

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
