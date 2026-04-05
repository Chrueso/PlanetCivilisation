using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Colonize")]
public class AIColonizeAction : AIAction
{
    public override float CalculateUtility(AIContext context) => (context.IsOnUninhabitedPlanet 
        && context.Model.EnoughShips(ShipType.Worker, context.GameConfig.MinWorkerShipNeededForColonize)) ? 1f : 0f;

    public override void Execute(AIContext context)
    {
        context.Controller.TryColonize(context.CurrentPlanet);
    }
}
