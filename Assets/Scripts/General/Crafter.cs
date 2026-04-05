using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mono.Cecil;
public class Crafter
{
    private StructureRecipeDatabaseSO structureRecipes;
    private EventBinding<GameStartEvent> gameStartBinding;
    private EventBinding<TurnChangeEvent> turnChangeEventBinding;
    //recipes

    public Crafter(StructureRecipeDatabaseSO structureRecipes)
    {
        this.structureRecipes = structureRecipes;
    }

    public bool TryBuildStructure(PlanetData planetData, StructureType structure, EntityModel entityModel)
    {
        if (CheckEntityInv(structure, out var recipe, entityModel))
        { 
            //planetData.BuildStructure(structure);
            foreach (var req in recipe)
            {
                entityModel.TakeResource(req.Key, req.Value);
            }
            return true;   
        }
        return false;
    }

    private bool CheckEntityInv(StructureType structure, out Dictionary<ResourceType, int> requirement, EntityModel entityModel)
    {
        bool canCraft = true;
        requirement = new();
        int index = (int)structure;
        StructureRecipeSO recipe = structureRecipes.Recipes[index];
        foreach (var structureRecipe in structureRecipes.Recipes)
        {
            if (structureRecipe.StructureType == structure)
            {
                for (int i = 0; i<structureRecipe.ResourceRequirement.Length-1; ++i)
                {
                    if (entityModel.Resources[structureRecipe.ResourceRequirement[i]] < structureRecipe.ResourceRequirementAmount[i])
                    {
                        canCraft = false;
                        return false;
                    }
                    requirement[structureRecipe.ResourceRequirement[i]] = structureRecipe.ResourceRequirementAmount[i];
                }
                break;
            }
        }
        
        return canCraft;
    }
}
