using System.Collections.Generic;

public class Crafter
{
    public StructureRecipeDatabaseSO StructureRecipes { get; private set; }

    public Crafter(StructureRecipeDatabaseSO structureRecipes)
    {
        this.StructureRecipes = structureRecipes;
    }

    public bool TryBuildStructure(StructureType structure, EntityModel entityModel)
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
        StructureRecipeSO recipe = StructureRecipes.Recipes[index];
        foreach (var structureRecipe in StructureRecipes.Recipes)
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
