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
    
    private IEntityController currentEntity;

    public Crafter(StructureRecipeDatabaseSO structureRecipes)
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(OnTurnChanged);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
        this.structureRecipes = structureRecipes;
    }

    private void OnTurnChanged(TurnChangeEvent turnChangeEvent)
    {
        
        
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        
    }
    public bool BuildStructure(PlanetData planetData, StructureType structure)
    {
        if (CheckEntityInv(structure, out var recipe))
        { 
            planetData.BuildStructure(structure);
            foreach (var req in recipe)
            {
                currentEntity.GetModel().TakeResource(req.Key, req.Value);
            }
            return true;   
        }
        return false;
    }

    private bool CheckEntityInv(StructureType structure, out Dictionary<ResourceType, int> requirement)
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
                    if (currentEntity.GetModel().Resources[structureRecipe.ResourceRequirement[i]] < structureRecipe.ResourceRequirementAmount[i])
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
