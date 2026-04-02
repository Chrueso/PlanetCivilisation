using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityModel 
{
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

    public EntityModel(ShipDatabaseSO shipDatabase, PlanetData homePlanet, FactionType factionType, int moveRadius = 5, int maxAP = 10, int startingResources = 10, int startingShips = 10, float yValue = 30)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        DiscoveredPlanets.Add(homePlanet);
        FactionType = factionType;
        Resources[ResourceType.Metals] = startingResources;
        Resources[ResourceType.Rations] = startingResources;
        Ships[ShipType.Scout] = startingShips;
        Ships[ShipType.Attacker] = startingShips;
        Ships[ShipType.Worker] = startingShips;

        MoveRadius = moveRadius;
        this.yValue = yValue;

        MaxAP = maxAP;
        CurrentAP = maxAP;

        OnResourcesChanged?.Invoke();
        OnShipsChanged?.Invoke();
        OnOwnedPlanetsChanged?.Invoke();
        OnDiscoveredPlanetsChanged?.Invoke();

        this.shipDatabase = shipDatabase;
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
            int increment = planet.StationedShips[ShipType.Worker];

            Resources[planet.PlanetResource[ResourceClass.Abundant]] += (planet.GeneratedResource[planet.PlanetResource[ResourceClass.Abundant]] + increment);
            Resources[planet.PlanetResource[ResourceClass.Scarce]] += (planet.GeneratedResource[planet.PlanetResource[ResourceClass.Scarce]] + increment);
        }

        Ships[ShipType.Scout] += 1;
        Ships[ShipType.Attacker] += 1;
        Ships[ShipType.Worker] += 1;

        OnResourcesChanged?.Invoke();
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
        if (Ships.ContainsKey(shipType))
        {
            Ships[shipType] += amount;
        }
        else
        {
            Ships.Add(shipType, amount);
        }

        OnShipsChanged?.Invoke();
    }

    public void RemoveShips(ShipType shipType, int amount) 
    {
        if (Ships.ContainsKey(shipType))
        {
            Ships[shipType] -= amount;

            if (Ships[shipType] <= 0)
            {
                Ships[shipType] = 0;
            }

            OnShipsChanged?.Invoke();
        }
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
}
