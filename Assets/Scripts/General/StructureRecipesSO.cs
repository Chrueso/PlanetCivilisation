using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StructureRecipesSO", menuName = "Scriptable Objects/StructureRecipesSO")]
public class StructureRecipesSO : ScriptableObject
{
    public StructureRecipeSO[] RECIPES;
}
