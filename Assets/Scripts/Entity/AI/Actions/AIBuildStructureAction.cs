using System.Collections.Generic;
using UnityEngine;

public class AIBuildStructureAction : AIAction
{
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

        StructureRecipeDatabaseSO recipeDatabase = context.Controller.Crafter.StructureRecipes;

        Dictionary<StructureType, float> structureScore = new Dictionary<StructureType, float>();
        foreach (var recipe in)
    }
}
