using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum ResourceClass
{
    Abundant,
    Scarce
}
public class PlanetData : IGridHexOccupant
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceClass, ResourceType> PlanetResource {  get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } // resources it generates
    public Dictionary<ResourceType, int> ResourceInventory { get; private set; } // Resource in inv, how many they have
    public FactionType FactionType { get; private set; }
    public List<StructureType> Structures { get; private set; }
    public Dictionary<ShipType, int> StationedShips { get; private set; } = new Dictionary<ShipType, int>();
    public Dictionary<FactionType, int> Affection {  get; private set; }
    public Dictionary<FactionType, RelationshipLevel> Relations {  get; private set; }
    public bool HasNAPact { get; private set; } = false;
    public GridHex CurrentHex { get; set; }
    

    public PlanetData(string planetName, FactionType faction, Dictionary<ResourceClass, ResourceType> resource, GridHex hex = null)
    {
        this.PlanetName = planetName;
        this.PlanetResource = resource;
        this.Resources = new Dictionary<ResourceType, int>() { {resource[ResourceClass.Abundant], 2}, { resource[ResourceClass.Scarce], 1 } };
        this.ResourceInventory = new Dictionary<ResourceType, int>() { { ResourceType.Metals,0 }, { ResourceType.Rations, 0 } };
        this.Relations = new();
        this.FactionType = faction;

        this.Structures = new List<StructureType>();

        this.StationedShips = new Dictionary<ShipType, int>() { 
            {ShipType.Scout, 0},
            {ShipType.Attacker, 0},
            {ShipType.Worker, 0 }
        };

        this.Affection = new Dictionary<FactionType, int>() {
            {FactionType.Human, 0 },
            {FactionType.DemiHuman, 0},
            {FactionType.IntelligentConstruct, 0},
        };

        this.CurrentHex = hex;
        UpdateRelations();
    }

    //public PlanetData(string planetName, Dictionary<ResourceType,int> resourceTypes, FactionType factionType, List<StructureType> structure, GridHex hex)
    //{
    //    this.PlanetName = planetName;

    //    foreach (KeyValuePair<ResourceType, int> kvp in resourceTypes)
    //    {
    //        this.Resources[kvp.Key] = kvp.Value;
    //    }

    //    //this.FactionType = factionType; // assign later since planets not owned
    //    this.Structures = structure.ToList();
    //    this.StationedShips = new Dictionary<ShipTypeSO, int>();
    //    this.Affection = new Dictionary<FactionType, int>() {
    //        {FactionType.Human, 0 },
    //        {FactionType.DemiHuman, 0},
    //        {FactionType.IntelligentConstruct, 0 },
    //    };
    //    this.CurrentHex = hex;
    //}

    public void SetFaction(FactionType factionType)
    {
        this.FactionType = factionType;
    }

    public void AddShips(ShipType shipType, int amount)
    {
        if (StationedShips.ContainsKey(shipType))
        {
            StationedShips[shipType] += amount;
        }
        else
        {
            StationedShips.Add(shipType, amount);
        }
    }

    public void RemoveShips(ShipType shipType, int amount)
    {
        if (StationedShips.TryGetValue(shipType, out var shipAmount))
        {
            StationedShips[shipType] -= Mathf.Max(0, shipAmount-amount);
        }
    }

    public int GetShipCount(ShipType shipType)
    {
        return StationedShips.ContainsKey(shipType) ? StationedShips[shipType] : 0;
    }

    // For attack and defense calculations, like Risk
    public int GetTotalAssaultShips()
    {
        int total = 0;
        foreach (var ship in StationedShips)
        {
            if (ship.Key == ShipType.Attacker)
            {
                total += ship.Value;
            }
        }
        return total;
    }

    //public void DebugPurposes()
    public void RaiseAffection(FactionType rizzler, int affection)
    {
        if (this.Affection.TryGetValue(rizzler, out int currAffection))
        {
            this.Affection[rizzler] = Math.Clamp(currAffection + affection, 0, 100);
            UpdateRelations();
        }
    }

    private void UpdateRelations()
    {
        foreach (var relations in Affection)
        {
            int affection = relations.Value;
            if (affection == 0 && affection <= 20)
            {
                Relations[relations.Key] = RelationshipLevel.HOSTILE;
            } else if (affection >= 21 && affection <= 49)
            {
                Relations[relations.Key] = RelationshipLevel.INDIFFERENT;
            } else if (affection >= 50 && affection <= 79)
            {
                Relations[relations.Key] = RelationshipLevel.NEUTRAL;
            } else if (affection >= 80 &&  affection <= 100)
            {
                Relations[relations.Key] = RelationshipLevel.FRIENDLY;
            }
        }
    }

    public void BuildStructure(StructureType structure)
    {
        if (this.Structures.Contains(structure)) return;
        this.Structures.Add(structure);
    }

    public void GainResource(ResourceType resource, int amount)
    {
        if (ResourceInventory.TryGetValue(resource, out var inventory))
        {
            ResourceInventory[resource] = Mathf.Clamp(inventory+amount, 0, 8000);
        }
    }

    public void RemoveResource(ResourceType resource, int amount)
    {
        if (ResourceInventory.TryGetValue(resource, out var inventory))
        {
            if (inventory < amount) return;
            ResourceInventory[resource] = Mathf.Max(inventory - amount, 0);
        }
    }

    public void AddPact(PactType pactType)
    {
        switch (pactType)
        {
            case PactType.NAP:
                HasNAPact = true;
                break;
            case PactType.FCP:
                //FactionType = GameManager.Instance.turnManager.currentFaction.FactionType;
                break;
        }
    }

    public void RemovePact(PactType pactType)
    {
        switch(pactType)
        {
            case PactType.NAP:
                HasNAPact = false;
                break;
        }
    }
}
