using UnityEngine;

[CreateAssetMenu(fileName = "StructureRecipeSO", menuName = "Scriptable Objects/StructureRecipeSO")]
public class StructureRecipeSO : ScriptableObject
{
    public StructureType StructureType;
    public ResourceType[] ResourceRequirement;
    public int[] ResourceRequirementAmount;
}
