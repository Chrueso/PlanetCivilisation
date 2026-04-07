using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/StationShip")]
public class AIStationShipAction : AIAction
{
    [SerializeField] private AnimationCurve maxShipCurve;

    public override float CalculateUtility(AIContext context)
    {
        if (context.IsOnOwnedPlanet)
        {
            int shipCount = context.Model.Ships[ShipType.Attacker] + context.Model.Ships[ShipType.Worker];
            int maxShipCount = context.GameConfig.MaxStationedAssaultShips + context.GameConfig.MaxStationedWorkerShips;
            int planetShipCount = context.CurrentPlanet.GetShipCount(ShipType.Attacker) + context.CurrentPlanet.GetShipCount(ShipType.Worker);

            if (planetShipCount <= Mathf.Round(shipCount * 0.2f))
            {
                return Mathf.Clamp01(maxShipCurve.Evaluate((float)shipCount / maxShipCount));
            }
        }

        return 0f;
    }

    public override void Execute(AIContext context)
    {
        context.Controller.TryStationShip(context.CurrentPlanet, ShipType.Worker, (int)Mathf.Round((float)context.Model.Ships[ShipType.Worker] * 0.2f));
        context.Controller.TryStationShip(context.CurrentPlanet, ShipType.Attacker, (int)Mathf.Round((float)context.Model.Ships[ShipType.Worker] * 0.2f));
    }
}
