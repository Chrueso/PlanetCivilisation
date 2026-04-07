using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StructureRecipesSO", menuName = "Scriptable Objects/StructureRecipesSO")]
public class StructureRecipeDatabaseSO
    : ScriptableObject
{
    public StructureRecipeSO[] Recipes;

    public Dictionary<StructureType, StructureRecipeSO> RecipeDict = new Dictionary<StructureType, StructureRecipeSO>();

    private void OnEnable()
    {
        foreach (var recipe in Recipes)
        {
            if (Recipes == null) continue;

            RecipeDict[recipe.StructureType] = recipe;
        }
    }

    public StructureRecipeSO GetShip(StructureType structureType)
    {
        if (RecipeDict.TryGetValue(structureType, out StructureRecipeSO recipe))
            return recipe;

        Debug.LogError($"Ship type {structureType} not found in database");
        return null;
    }
}
