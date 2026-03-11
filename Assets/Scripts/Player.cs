using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGridHexOccupant
{
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipType, int> Ships { get; private set; } = new Dictionary<ShipType, int>();
    public FactionType FactionType { get; private set; }
    public List<PlanetData> OwnedPlanets { get; private set; } = new List<PlanetData>();
    public List<PlanetData> DiscoveredPlanets { get; private set; } = new List<PlanetData>();
    public PlanetData HomePlanet { get; private set; }

    public GridHex CurrentHex { get; set; }

    float playerY = 3f;

    public void Init(PlanetData homePlanet, FactionType factionType)
    {
        HomePlanet = homePlanet;
        OwnedPlanets.Add(homePlanet);
        DiscoveredPlanets.Add(homePlanet);
        FactionType = factionType;
        CurrentHex = homePlanet.CurrentHex;
        transform.position = new Vector3(CurrentHex.WorldPosition.x, playerY, CurrentHex.WorldPosition.z);
        this.Resources[ResourceType.Metals] = 10;
        this.Resources[ResourceType.Rations] = 10;
        this.Resources[ResourceType.Energy_Source] = 10;
        Ships[ShipType.Scout] = 10;
        Ships[ShipType.Attacker] = 10;
        Ships[ShipType.Worker] = 10;
    }

    public void CalculateResourceGain()
    {
        foreach (var planet in OwnedPlanets)
        {
            int increment = planet.StationedShips[ShipType.Worker];
            if (planet.Structures.Contains(StructureType.Extractor))
            {
                this.Resources[ResourceType.Metals] += (1 + increment);
                this.Resources[ResourceType.Rations] += (1 + increment);
                this.Resources[ResourceType.Energy_Source] += (1 + increment);
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

    public void TakeResource(ResourceType resource)
    {
        if (this.Resources.TryGetValue(resource, out int amount))
        {
            this.Resources[resource] = amount - 1;
        }
    }
    public void GainResource(ResourceType resource)
    {
        if (this.Resources.TryGetValue(resource, out int amount))
        {
            this.Resources[resource] = amount + 1;
        }
    }

    public void MoveToHex(GridHex hex)
    {
        CurrentHex = hex;
        transform.position = new Vector3(CurrentHex.WorldPosition.x, playerY, CurrentHex.WorldPosition.z); ;
        //Chnage to some tween / coroutine / move thing action yes
    }

}
