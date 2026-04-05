using UnityEngine;

public struct GetShipAfterTurnsPayload 
{
    public bool Active;
    public int ShipAmount;
    public ShipType ShipToBeBuilt;

    public GetShipAfterTurnsPayload(int throwaway)
    {
        this.Active = false;
        this.ShipAmount = 0;
        this.ShipToBeBuilt = ShipType.Worker;
    }
    public GetShipAfterTurnsPayload(int shipAmount, ShipType ship)
    {
        this.Active = true;
        this.ShipAmount = shipAmount;
        this.ShipToBeBuilt = ship;
    }

    public void DecrementAmount()
    {
        this.ShipAmount--;
        if (this.ShipAmount <= 0)
        {
            this.Active = false;
        }
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
