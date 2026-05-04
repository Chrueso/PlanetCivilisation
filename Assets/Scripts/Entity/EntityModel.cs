using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EntityModel 
{
    private GameConfigSO gameConfig;
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipType, int> Ships { get; private set; } = new Dictionary<ShipType, int>();
    public FactionType FactionType { get; private set; }
    public HashSet<PlanetData> OwnedPlanets { get; private set; } = new HashSet<PlanetData>();
    public HashSet<PlanetData> DiscoveredPlanets { get; private set; } = new HashSet<PlanetData>();
    public HashSet<GridHex> DiscoveredHexes { get; private set; } = new HashSet<GridHex>();
    public PlanetData HomePlanet { get; private set; }
    private GridHex currentHex;
    public GridHex CurrentHex
    {
        get => currentHex;
        set
        {
            currentHex = value;
            OnCurrentHexChanged?.Invoke();
        }
    }

    public int MoveRadius { get; private set; }
    public float yValue { get; private set; }

    public int MaxAP { get; private set; }
    public int CurrentAP { get; private set; }

    public event Action OnResourcesChanged;
    public event Action OnShipsChanged;
    public event Action OnOwnedPlanetsChanged;
    public event Action OnDiscoveredPlanetsChanged;
    public event Action OnCurrentHexChanged;
    public event Action OnAPChanged;

    private ShipDatabaseSO shipDatabase;

    public EntityModel(ShipDatabaseSO shipDatabase, PlanetData homePlanet, FactionType factionType, GameConfigSO gameConfig, float yValue = 30)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        DiscoveredPlanets.Add(homePlanet);
        FactionType = factionType;
        Resources[ResourceType.Metals] = gameConfig.StartingResourcesAmount;
        Resources[ResourceType.Rations] = gameConfig.StartingResourcesAmount;
        Ships[ShipType.Scout] = gameConfig.StartingShipsAmount;
        Ships[ShipType.Attacker] = gameConfig.StartingShipsAmount;
        Ships[ShipType.Worker] = gameConfig.StartingShipsAmount;

        MoveRadius = gameConfig.MoveRadius;
        this.yValue = yValue;

        MaxAP = gameConfig.MaxAP;
        CurrentAP = gameConfig.MaxAP;

        OnResourcesChanged?.Invoke();
        OnShipsChanged?.Invoke();
        OnOwnedPlanetsChanged?.Invoke();
        OnDiscoveredPlanetsChanged?.Invoke();

        this.shipDatabase = shipDatabase;
        this.gameConfig = gameConfig;
    }

    public void AddAP(int amount)
    {
        CurrentAP = Mathf.Clamp(CurrentAP + amount, 0, MaxAP);
        OnAPChanged?.Invoke();
    }

    public void RemoveAP(int amount)
    {
        CurrentAP = Mathf.Max(0, CurrentAP - amount);
        OnAPChanged?.Invoke();
    }

    public void RefreshAP()
    {
        CurrentAP = MaxAP;
        OnAPChanged?.Invoke();
    }

    public void AddDiscoveredHex(GridHex hex)
    {
        DiscoveredHexes.Add(hex);
    }

    public void CalculateResourceGain()
    {
        foreach (var planet in OwnedPlanets)
        {
            int increment = planet.StationedShips[ShipType.Worker] + gameConfig.WorkerShipEfficacyValue;
            int mult = planet.Structures.Contains(StructureType.Extractor) ? gameConfig.ExtractorResourceMultiplier : 1;
            Resources[planet.PlanetResource[ResourceClass.Abundant]] += ((planet.GeneratedResource[planet.PlanetResource[ResourceClass.Abundant]]  * mult) + increment);
            Resources[planet.PlanetResource[ResourceClass.Scarce]] += ((planet.GeneratedResource[planet.PlanetResource[ResourceClass.Scarce]] * mult) + increment);
            if (planet.Structures.Contains(StructureType.Shipyard))
            {
                AddShips(ShipType.Scout, 1);
                AddShips(ShipType.Attacker, 1);
                AddShips(ShipType.Worker, 1);
            }
            //if (!planet.CheckPlanetShipyardActive()) continue;
            //GetShipAfterTurnsPayload payload = planet.GetPlanetShipyardPayload();
            //if (!TryBuildShip(planet, payload.ShipToBeBuilt)) continue;
            
        }   
        Debug.Log($"Scouts : {Ships[ShipType.Scout]} | Assaults : {Ships[ShipType.Attacker]} | Workers : {Ships[ShipType.Worker]}");
        OnResourcesChanged?.Invoke();
    }

    public bool TryBuildShip(PlanetData planet, ShipType payload)
    {
        ShipDataSO shipData = shipDatabase.GetShip(payload);
        Dictionary<ResourceType, int> costQueue = new();
        foreach (var resource in shipData.RequiredResources)
        {
            if (Resources[resource.ResourceType] < resource.Amount) return false;
            costQueue[resource.ResourceType] = resource.Amount;
        }

        foreach (var cost in costQueue)
        {
            TakeResource(cost.Key, cost.Value);
        }
        AddShips(payload, 1);
        planet.ProgressShipBuilding();
        Debug.Log($"WOI LOOK AT THIS{planet.GetPlanetShipyardPayload().Active}");
        return true;
    }

    public int CalculateAttackPower()
    {
        if (!Ships.TryGetValue(ShipType.Attacker, out int count))
            return 0;

        var data = shipDatabase.GetShip(ShipType.Attacker);
        return count * data.AttackPower;
    }

    public void AddShips(ShipType shipType, int amount)
    {
        if (Ships.TryGetValue(shipType, out int shipAmount)) {
            int max = 1;
            switch (shipType)
            {
                case ShipType.Scout:
                    max = gameConfig.MaxHeldScoutShips;
                    break;
                case ShipType.Attacker:
                    max = gameConfig.MaxHeldAssaultShips;
                    break;
                case ShipType.Worker:
                    max = gameConfig.MaxHeldWorkerShips;
                    break;
            }
            Ships[shipType] = Mathf.Clamp(shipAmount + amount, 0, max);
        } else
        {
            Ships.Add(shipType, amount);
        }
        OnShipsChanged?.Invoke();
    }

    public void RemoveShips(ShipType shipType, int amount) 
    {
        if (Ships.TryGetValue(shipType, out int shipAmount))
        {
            Ships[shipType] = Mathf.Max(shipAmount-amount, 0);
        }
        OnShipsChanged?.Invoke();
    }

    public bool EnoughShips(ShipType shipType, int amount)
    {
        if (Ships.ContainsKey(shipType))
        {
            return Ships[shipType] >= amount;
        }
        return false;
    }

    public void AddPlanetDiscovery(PlanetData planet)
    {
        if (!DiscoveredPlanets.Contains(planet))
        {
            DiscoveredPlanets.Add(planet);
            OnDiscoveredPlanetsChanged?.Invoke();

        }
    }

    public void AddOwnedPlanets(PlanetData planet)
    {
        if (!OwnedPlanets.Contains(planet))
        {
            OwnedPlanets.Add(planet);
            OnOwnedPlanetsChanged?.Invoke();
        }
    }

    public void RemoveOwnedPlanets(PlanetData planet)
    {
        if (OwnedPlanets.Contains(planet))
        {
            OwnedPlanets.Remove(planet);
            OnOwnedPlanetsChanged?.Invoke();
        }
    }

    public void TakeResource(ResourceType resource, int amount)
    {
        if (Resources.TryGetValue(resource, out int invAmount))
        {
            Resources[resource] = Mathf.Max(invAmount - amount, 0);
            OnResourcesChanged?.Invoke();
        }
    }
    public void GainResource(ResourceType resource, int amount)
    {
        if (Resources.TryGetValue(resource, out int invAmount))
        {
            Resources[resource] = invAmount + amount;
            OnResourcesChanged?.Invoke();
        }
    }

    public int GetShipTotal()
    {
        int total = 0;
        foreach (var ship in Ships)
        {
            total += ship.Value;
        }

        return total;
    }
}
