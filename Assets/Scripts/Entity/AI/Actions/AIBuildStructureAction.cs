using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/BuildStructure")]
public class AIBuildStructureAction : AIAction
{
    [SerializeField] private AnimationCurve shipyardCurve; //if lower ships more likely to build a shipyard

    public override float CalculateUtility(AIContext context)
    {
        return (context.IsOnOwnedPlanet && context.HasResourceToBuildStructures && context.CurrentPlanet.Structures.Count == 0) ? 1f : 0f;
    }

    public override void Execute(AIContext context)
    {
        //Default is extractor
        //If ships low then shipyard
        //Teleporter not used
        //Defense idk yet

        //Dictionary<StructureType, float> structureScore = new Dictionary<StructureType, float>();
 
        //structureScore.Add(StructureType.Extractor, 0.5f);

        //I dont count scout ships cause im sigma
        int desiredShipCount = context.GameConfig.MaxHeldAssaultShips + context.GameConfig.MaxHeldWorkerShips;
        int currentShipCount = context.Model.Ships[ShipType.Attacker] + context.Model.Ships[ShipType.Worker];

        float shipyardScore = shipyardCurve.Evaluate((float)currentShipCount / desiredShipCount);
        //structureScore.Add(StructureType.Shipyard, shipyardScore);

        //float bestScore = 0.5f;

        //foreach (var kvp in structureScore)
        //{

        //}
        StructureType bestStructure = shipyardScore > 0.5f ? StructureType.Shipyard : StructureType.Extractor;
        context.Controller.TryBuildStructure(context.CurrentPlanet, bestStructure);
    }
}
