using System;
using System.Collections.Generic;
using System.Linq;

public class PlanetData 
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public FactionType FactionType { get; private set; }
    public List<Structure> Structures { get; private set; }
    public Dictionary<ShipTypeSO, int> StationedShips { get; private set; }
    public Dictionary<FactionType, int> Affection {  get; private set; }
    public PlanetData(string planetName, FactionType faction)
    {
        this.PlanetName = planetName;
        this.Resources = new Dictionary<ResourceType, int>();
        this.FactionType = faction;
        this.Structures = new List<Structure>();
        this.StationedShips = new Dictionary<ShipTypeSO, int>();
        this.Affection = new Dictionary<FactionType, int>() {
            {FactionType.Human, 0 },
            {FactionType.DemiHuman, 0},
            {FactionType.IntelligentConstruct, 0 },
        };
    }

    public PlanetData(string planetName, Dictionary<ResourceType,int> resourceTypes, FactionType factionType, List<Structure> structure)
    {
        this.PlanetName = planetName;

        foreach (KeyValuePair<ResourceType, int> kvp in resourceTypes)
        {
            this.Resources[kvp.Key] = kvp.Value;
        }

        this.FactionType = factionType;
        this.Structures = structure.ToList();
        this.StationedShips = new Dictionary<ShipTypeSO, int>();
        this.Affection = new Dictionary<FactionType, int>() {
            {FactionType.Human, 0 },
            {FactionType.DemiHuman, 0},
            {FactionType.IntelligentConstruct, 0 },
        };
    }

    public void RaiseAffection(FactionType rizzler, int affection)
    {
        if (this.Affection.TryGetValue(rizzler, out int currAffection))
        {
            this.Affection[rizzler] = Math.Clamp(currAffection + affection, 0, 100);
        }
    }

    public void BuildStructure(Structure structure)
    {
        if (this.Structures.Contains(structure)) return;
        this.Structures.Add(structure);
    }

    public void StationShips(Dictionary<ShipTypeSO, int> stationShips)
    {
        foreach (KeyValuePair<ShipTypeSO, int> kvp in stationShips)
        {
            if (this.StationedShips.ContainsKey(kvp.Key))
            {
                this.StationedShips[kvp.Key] += kvp.Value;
                continue;
            }
            this.StationedShips[kvp.Key] = kvp.Value;
        }
    }
}
