using System.Collections.Generic;
using System;

public class EntityModel 
{
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipType, int> Ships { get; private set; } = new Dictionary<ShipType, int>();
    public FactionType FactionType { get; private set; }
    public List<PlanetData> OwnedPlanets { get; private set; } = new List<PlanetData>();
    public List<PlanetData> DiscoveredPlanets { get; private set; } = new List<PlanetData>();
    public PlanetData HomePlanet { get; private set; }

    public event Action OnCurrentHexChanged;
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

    public EntityModel(PlanetData homePlanet, FactionType factionType, int moveRadius = 5, float yValue = 30)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        DiscoveredPlanets.Add(homePlanet);
        FactionType = factionType;
        CurrentHex = homePlanet.CurrentHex;
        Resources[ResourceType.Metals] = 10;
        Resources[ResourceType.Rations] = 10;
        Resources[ResourceType.Credits] = 10;
        Ships[ShipType.Scout] = 10;
        Ships[ShipType.Attacker] = 10;
        Ships[ShipType.Worker] = 10;

        MoveRadius = moveRadius;
        this.yValue = yValue;
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
    }

    public void StationShips(Dictionary<ShipType, int> stationShips)
    {
        foreach (var kvp in stationShips)
        {
            if (this.Ships.ContainsKey(kvp.Key))
            {
                this.Ships[kvp.Key] -= kvp.Value;
                continue;
            }
            this.Ships[kvp.Key] = kvp.Value;
        }
    }

    public void AddPlanetDiscovery(PlanetData planet)
    {
        if (!DiscoveredPlanets.Contains(planet))
        {
            DiscoveredPlanets.Add(planet);
        }
    }

    public void AddOwnedPlanets(PlanetData planet)
    {
        if (!OwnedPlanets.Contains(planet))
        {
            OwnedPlanets.Add(planet);
        }
    }

    public void RemoveOwnedPlanets(PlanetData planet)
    {
        if (OwnedPlanets.Contains(planet))
        {
            OwnedPlanets.Remove(planet);
        }
    }

    public void TakeResource(ResourceType resource)
    {
        if (Resources.TryGetValue(resource, out int amount))
        {
            Resources[resource] = amount - 1;
        }
    }
    public void GainResource(ResourceType resource)
    {
        if (Resources.TryGetValue(resource, out int amount))
        {
            Resources[resource] = amount + 1;
        }
    }
}
