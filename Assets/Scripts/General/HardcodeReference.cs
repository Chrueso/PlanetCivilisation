using System.Collections.Generic;
using UnityEngine;

public class HardcodeReference : Singleton<HardcodeReference>
{
    [SerializeField] private ShipTypeSO scoutShip;
    [SerializeField] private ShipTypeSO attackShip;
    [SerializeField] private ShipTypeSO workerShip;

    public ShipTypeSO ScoutShip => scoutShip;
    public ShipTypeSO AttackShip => attackShip;
    public ShipTypeSO WorkerShip => workerShip;
    
}
