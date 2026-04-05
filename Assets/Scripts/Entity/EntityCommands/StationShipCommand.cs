using UnityEngine;
using System;

public class StationShipCommand : ICommand
{
    EntityModel entityModel;
    private Action onComplete;
    EntityController entityController;
    PlanetData targetPlanet;
    ShipType shipType;
    int amount;

    public StationShipCommand(EntityController entityController, PlanetData targetPlanet, ShipType shipData, int amount, Action onComplete)
    {
        this.entityController = entityController;
        this.targetPlanet = targetPlanet;
        this.shipType = shipData;
        this.onComplete = onComplete;
        this.entityModel = entityController.GetModel();
        this.amount = amount;
    }

    public void Execute()
    {
        entityController.IsPerformingAction = true;

        entityModel.RemoveShips(shipType, amount);
        targetPlanet.AddShips(shipType, amount);

        entityController.IsPerformingAction = false;
        onComplete?.Invoke();

        Debug.Log(this.ToString());
    }

    public override string ToString() =>
        $"{entityModel.FactionType} has stationed {amount} {shipType} on planet {targetPlanet.PlanetName}";
}
