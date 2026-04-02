
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipModelEntry
{
    public ShipType shipType;
    public FactionType faction;
    public GameObject modelPrefab;
}

[CreateAssetMenu(fileName = "ShipModelDatabaseSO", menuName = "Scriptable Objects/ShipModelDatabaseSO")]
public class ShipModelDatabaseSO : ScriptableObject
{
    [SerializeField] List<ShipModelEntry> modelEntries = new List<ShipModelEntry>();

    public Dictionary<(FactionType, ShipType), GameObject> models;

    void OnEnable()
    {
       Init();
    }

    void Init()
    {
        models = new();

        foreach (var modelEntry in modelEntries)
        {
            var key = (modelEntry.faction,modelEntry.shipType);
            models[key] = modelEntry.modelPrefab;
        }
    }

    public GameObject GetModel(FactionType faction, ShipType ship)
    {
        if (models.TryGetValue((faction, ship), out var model)) return model;
        Debug.Log($"no model for {faction} {ship} found");
        return null;
    }

}



