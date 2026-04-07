using System;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceClass
{
    Abundant,
    Scarce
}
public class PlanetData : IGridHexObject
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceClass, ResourceType> PlanetResource {  get; private set; }
    public Dictionary<ResourceType, int> GeneratedResource { get; private set; } // resources it generates
    public Dictionary<ResourceType, int> ResourceInventory { get; private set; } // Resource in inv, how many they have
    public FactionType FactionType { get; private set; }
    public List<StructureType> Structures { get; private set; }
    public Dictionary<ShipType, int> StationedShips { get; private set; } 
    public Dictionary<FactionType, int> Affection {  get; private set; }
    public Dictionary<FactionType, RelationshipLevel> Relations {  get; private set; }
    public bool HasNAPact { get; private set; } = false;

    public GridHex CurrentHex { get; set; }
    public bool IsHiddenForPlayer { get; private set; }

    private ShipDatabaseSO shipDatabase;

    private GameConfigSO gameConfig;
    public PlanetView View { get; private set; }

    public PlanetData(PlanetView view, ShipDatabaseSO shipDatabase, string planetName, FactionType faction, Dictionary<ResourceClass, ResourceType> resource, GameConfigSO gameConfig, GridHex hex = null)
    {
        this.View = view;
        this.shipDatabase = shipDatabase;

        this.PlanetName = planetName;
        this.PlanetResource = resource;
        this.GeneratedResource = new Dictionary<ResourceType, int>() {
            {resource[ResourceClass.Abundant], UnityEngine.Random.Range(gameConfig.MinAbundantResourceGen, gameConfig.MaxAbundantResourceGen)},
            { resource[ResourceClass.Scarce], UnityEngine.Random.Range(gameConfig.MinScarceResourceGen, gameConfig.MaxScarceResourceGen) } };
        this.ResourceInventory = new Dictionary<ResourceType, int>() { { ResourceType.Metals, 5 }, { ResourceType.Rations, 5 } }; // to test trade
        this.Relations = new();
        this.FactionType = faction;

        this.Structures = new List<StructureType>();

        this.StationedShips = new Dictionary<ShipType, int>() { 
            {ShipType.Scout, 0},
            {ShipType.Attacker, 5},
            {ShipType.Worker, 3 }
        };

        this.Affection = new Dictionary<FactionType, int>() {
            {FactionType.Human, 0 },
            {FactionType.DemiHuman, 0},
            {FactionType.IntelligentConstruct, 0},
        };

        this.CurrentHex = hex;
        this.gameConfig = gameConfig;
        UpdateRelations();
    }

    public void Show()
    {
        IsHiddenForPlayer = false;
        View.Show();
    }

    public void Hide()
    {
        IsHiddenForPlayer = true;
        View.Hide();
    }
    
    public void SetFaction(FactionType factionType)
    {
        this.FactionType = factionType;
    }

    public void AddShips(ShipType shipType, int amount)
    {
        if (StationedShips.TryGetValue(shipType, out int shipAmount))
        {
            int max = 1;
            switch (shipType)
            {
                case ShipType.Attacker:
                    max = gameConfig.MaxStationedAssaultShips;
                    break;
                case ShipType.Worker:
                    max = gameConfig.MaxStationedWorkerShips;
                    break;
            }
            StationedShips[shipType] = Mathf.Clamp(shipAmount + amount, 0, max);
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

    public int CalculateDefensePower()
    {
        int defensePower = 0;
        foreach (var kvp in StationedShips)
        {
            ShipType shipType = kvp.Key;
            int count = kvp.Value;
            ShipDataSO shipData = shipDatabase.GetShip(shipType);
            defensePower += shipData.AttackPower * count;
        }

        return defensePower;
    }

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
