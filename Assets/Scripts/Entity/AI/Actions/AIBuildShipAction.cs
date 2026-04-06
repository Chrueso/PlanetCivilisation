using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/BuildShip")]
public class AIBuildShipAction : AIAction
{

    public override float CalculateUtility(AIContext context)
    {
        if (context.IsOnOwnedPlanet)
        {
            GameConfigSO gameConfig = context.GameConfig;
            float utility = context.Model.Ships[ShipType.Worker] < gameConfig.MaxHeldWorkerShips / 2f
                || context.Model.Ships[ShipType.Attacker] < gameConfig.MaxHeldAssaultShips / 2f
                || context.Model.Ships[ShipType.Scout] < gameConfig.MaxHeldScoutShips / 2f
                ? 1f : 0f;
            return utility;
        }
        return 0f;
    }

    public override void Execute(AIContext context)
    {
        Dictionary<ShipType, float> shipWeightage = new();
        shipWeightage[ShipType.Scout] = (context.Model.Ships[ShipType.Scout] / context.GameConfig.MaxHeldScoutShips) + 0.1f; // lowest priority
        shipWeightage[ShipType.Attacker] = (context.Model.Ships[ShipType.Attacker] / context.GameConfig.MaxHeldAssaultShips) + 1f; // highest priority
        shipWeightage[ShipType.Worker] = (context.Model.Ships[ShipType.Worker] / context.GameConfig.MaxHeldWorkerShips) + 0.5f; // medium priority
        float winner = 0f;
        ShipType ship = ShipType.Worker;
        foreach (var ships in shipWeightage)
        {
            if (ships.Value > winner)
            {
                winner = ships.Value;
                ship = ships.Key;
            }
        }

        context.Controller.TryBuildShip(context.CurrentPlanet, ship, 1);
    }
}
