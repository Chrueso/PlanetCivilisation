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
    

    
}
