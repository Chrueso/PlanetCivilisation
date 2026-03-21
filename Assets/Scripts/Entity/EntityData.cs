using System.Collections.Generic;
using System;

public class EntityData 
{
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipType, int> Ships { get; private set; } = new Dictionary<ShipType, int>();
    public FactionType FactionType { get; private set; }
    public List<PlanetData> OwnedPlanets { get; private set; } = new List<PlanetData>();
    public List<PlanetData> DiscoveredPlanets { get; private set; } = new List<PlanetData>();
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

    public event Action OnResourcesChanged;
    public event Action OnShipsChanged;
    public event Action OnOwnedPlanetsChanged;
    public event Action OnDiscoveredPlanetsChanged;
    public event Action OnCurrentHexChanged;
    
    public EntityData(PlanetData homePlanet, FactionType factionType, int moveRadius = 5, float yValue = 30)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        DiscoveredPlanets.Add(homePlanet);
        FactionType = factionType;
        Resources[ResourceType.Metals] = 10;
        Resources[ResourceType.Rations] = 10;
        Resources[ResourceType.Credits] = 10;
        Ships[ShipType.Scout] = 10;
        Ships[ShipType.Attacker] = 10;
        Ships[ShipType.Worker] = 10;

        MoveRadius = moveRadius;
        this.yValue = yValue;

        OnResourcesChanged?.Invoke();
        OnShipsChanged?.Invoke();
        OnOwnedPlanetsChanged?.Invoke();
        OnDiscoveredPlanetsChanged?.Invoke();
    }



    public void CalculateResourceGain()
    {
        foreach (var planet in OwnedPlanets)
        {
            int increment = planet.StationedShips[ShipType.Worker];
            if (planet.Structures.Contains(StructureType.Extractor))
            {
                Resources[ResourceType.Metals] += (1 + increment);
                Resources[ResourceType.Rations] += (1 + increment);
                Resources[ResourceType.Credits] += (1 + increment);
            }
            if (planet.Structures.Contains(StructureType.Shipyard))
            {
                Ships[ShipType.Scout] += (1 + increment);
                Ships[ShipType.Attacker] += (1 + increment);
                Ships[ShipType.Worker] += (1 + increment);
            }
        }

        OnResourcesChanged?.Invoke();
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

    public void TakeResource(ResourceType resource)
    {
        if (Resources.TryGetValue(resource, out int amount))
        {
            Resources[resource] = amount - 1;
            OnResourcesChanged?.Invoke();
        }
    }
    public void GainResource(ResourceType resource)
    {
        if (Resources.TryGetValue(resource, out int amount))
        {
            Resources[resource] = amount + 1;
            OnResourcesChanged?.Invoke();
        }
    }
}
