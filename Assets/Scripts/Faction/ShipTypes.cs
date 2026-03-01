using System;
using UnityEngine;
public enum ShipActionType
{
    Combat,      // Can engage in battles
    Scout,       // Can reveal fog of war / discover planets
    Resource,    // Can gather/transport resources
    Construction // Can build structures
}

[CreateAssetMenu(fileName = "New Ship Type", menuName = "Ships/Ship Type")]
public class ShipTypeSO : ScriptableObject
{
    [Header("Ship Main Stats")]
    [SerializeField] private string shipName;
    [SerializeField] private Sprite shipIcon;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private ShipActionType actionType;

    [Header("Ship Build Requirements")]
    [SerializeField] private ResourceRequirement[] requiredResources;
    [SerializeField] private float buildTime = 5f;


    // Public accessors (idk whatever u guys need just add)
    // public int BuildCost => ;
}

[System.Serializable]
public class ResourceRequirement
{
    public ResourceType resourceType;
    public int amount;
}