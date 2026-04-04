using UnityEngine;

public struct GetShipAfterTurnsPayload 
{
    public int ShipAmount;
    public ShipType ShipToBeBuilt;

    public GetShipAfterTurnsPayload(int shipAmount, ShipType ship)
    {
        this.ShipAmount = shipAmount;
        this.ShipToBeBuilt = ship;
    }

    public void DecrementAmount()
    {
        this.ShipAmount--;
    }
}

public class Structures
{
    // Shipyard utility functions
    public static GetShipAfterTurnsPayload BuildScoutShip(int amount)
    {
        return new(amount, ShipType.Scout);
    }

    public static GetShipAfterTurnsPayload BuildAssaultShip(int amount)
    {
        return new(amount, ShipType.Attacker);
    }

    public static GetShipAfterTurnsPayload BuildWorkerShip(int amount)
    {
        return new(amount, ShipType.Worker);
    }
}
