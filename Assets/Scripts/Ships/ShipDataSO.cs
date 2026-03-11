using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Data", menuName = "Ships/Ship Data")]
public class ShipDataSO : ScriptableObject
{
    [Header("Ship Main Stats")]
    public Sprite Icon;
    public GameObject Prefab;
    public ShipType Type;
    public int AttackPower = 1; 

    [Header("Ship Build Requirements")]
    public ResourceRequirement[] RequiredResources;
    public float BuildTime = 5f;

}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceType ResourceType;
    public int Amount;
}