using UnityEngine;

public class AIBuildShipAction : AIAction
{
    [SerializeField] private AnimationCurve maxShipCurve;

    public override float CalculateUtility(AIContext context)
    {
        if (context.IsOnOwnedPlanet)
        {
            int shipCount = context.Model.Ships[ShipType.Attacker] + context.Model.Ships[ShipType.Worker];
            int maxShipCount = context.GameConfig.MaxStationedAssaultShips + context.GameConfig.MaxStationedWorkerShips;
            int planetShipCount = context.CurrentPlanet.GetShipCount(ShipType.Attacker) + context.CurrentPlanet.GetShipCount(ShipType.Worker);

            if (context.CurrentPlanet.Structures.Contains(StructureType.Shipyard) && context.HasResourceToBuildShip)
            {
                return 1 - Mathf.Clamp01(maxShipCurve.Evaluate((float)shipCount / maxShipCount));
            }
        }

        return 0f;
    }

    public override void Execute(AIContext context)
    {
        //Choose what ship to build?
        ShipType ship = ShipType.Attacker;
        if (context.Model.Ships[ShipType.Attacker] < context.GameConfig.MaxHeldAssaultShips)
        {
            ship = ShipType.Attacker;
        } else if (context.Model.Ships[ShipType.Worker] < context.GameConfig.MaxHeldWorkerShips)
        {
            ship = ShipType.Worker;
        } else if (context.Model.Ships[ShipType.Scout] < context.GameConfig.MaxHeldScoutShips)
        {
            ship = ShipType.Scout;
        }
        context.Controller.TryBuildShip(context.CurrentPlanet, ship, 1);
    }
}
