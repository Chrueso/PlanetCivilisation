using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StructureRecipeSO", menuName = "Scriptable Objects/StructureRecipeSO")]
public class StructureRecipeSO : ScriptableObject
{
    public StructureType StructureType;
    public ResourceType[] ResourceRequirement;
    public int[] ResourceRequirementAmount;

    public Dictionary<ResourceType, int> RecipeDict { get; private set; }

    private void OnEnable()
    {
        if (ResourceRequirement.Length != ResourceRequirementAmount.Length)
        {
            Debug.LogWarning($"{name}: array length mismatch!");
            return;
        }

        RecipeDict = new Dictionary<ResourceType, int>();
        for (int i = 0; i < ResourceRequirement.Length; i++)
        {
            RecipeDict[ResourceRequirement[i]] = ResourceRequirementAmount[i];
        }
    }
}


