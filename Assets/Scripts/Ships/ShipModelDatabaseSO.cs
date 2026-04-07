
using System.Collections.Generic;
using UnityEngine;

public enum shipMergeTier
{ 
    single,
    grouped
}

[System.Serializable]
public class ShipModelEntry
{
    public ShipType shipType;
    public shipMergeTier shipTier;
    public FactionType faction;
    public GameObject modelPrefab;
}

[CreateAssetMenu(fileName = "ShipModelDatabaseSO", menuName = "Scriptable Objects/ShipModelDatabaseSO")]
public class ShipModelDatabaseSO : ScriptableObject
{
    [SerializeField] List<ShipModelEntry> modelEntries = new List<ShipModelEntry>();

    public Dictionary<(FactionType, ShipType, shipMergeTier), GameObject> models;

    void OnEnable()
    {
       Init();
    }

    void Init()
    {
        models = new();

        foreach (var modelEntry in modelEntries)
        {
            var key = (modelEntry.faction,modelEntry.shipType, modelEntry.shipTier);
            models[key] = modelEntry.modelPrefab;
        }
    }

    public GameObject GetModel(FactionType faction, ShipType ship, shipMergeTier shipTier)
    {
        if (models.TryGetValue((faction, ship, shipTier), out var model)) return model;
        Debug.Log($"no model for {faction} {shipTier} {ship} found");
        return null;
    }

}



