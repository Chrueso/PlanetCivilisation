using System.Collections.Generic;
using UnityEngine;

public interface IEntityController
{
    public bool IsCurrentTurn { get; }

    public bool IsActivePlayer { get; set; }

    public HashSet<GridHex> HexesInMoveRadius { get; }

    public EntityModel GetModel();

    public EntityView GetView();

    public void HandleCurrrentHexChanged();

    public bool CheckIfHexIsInMoveRadius(GridHex hex);

    public void UpdateHexesInMoveRadius();

    public void UpdateDiscoveredHex();

    public void UpdateVision();

    public bool CanExecuteAction();

    public bool TryEndTurn();

    public bool TryMove(GridHex hex);
    public bool TryMoveScoutShip(GridHex hex, EntityScoutShipView scoutShipView);

    public bool TryColonize(PlanetData planet);

    public bool TryAttack(PlanetData planet);

    public bool TryBuildStructure(PlanetData planet, StructureType structure);
    public bool TryBuildShip(ShipType ship, int amount);
    public bool TryStationShip(PlanetData planet, ShipType shipType, int amount);

    // Diplomacy
    public void Trade(PlanetData planet);

    public void Gift(PlanetData planet);

    public void Agreement(PlanetData planet);


}
