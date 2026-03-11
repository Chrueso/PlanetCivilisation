using System;
using System.Collections.Generic;
using System.Linq;

public class PlanetData : IGridHexOccupant
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public FactionType FactionType { get; private set; }
    public List<StructureType> Structures { get; private set; }
    public Dictionary<ShipType, int> StationedShips { get; private set; } = new Dictionary<ShipType, int>();
    public Dictionary<FactionType, int> Affection {  get; private set; }
    public GridHex CurrentHex { get; set; }

    public PlanetData(string planetName, FactionType faction, GridHex hex = null)
    {
        this.PlanetName = planetName;
        this.Resources = new Dictionary<ResourceType, int>();
        this.FactionType = faction;

        this.Structures = new List<StructureType>();

        this.StationedShips = new Dictionary<ShipType, int>() { 
            {ShipType.Scout, 0},
            {ShipType.Attacker, 0 },
            {ShipType.Worker, 0 }
        };

        this.Affection = new Dictionary<FactionType, int>() {
            {FactionType.Human, 0 },
            {FactionType.DemiHuman, 0},
            {FactionType.IntelligentConstruct, 0 },
        };

        this.CurrentHex = hex;
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
        if (shipType == null || amount <= 0)
        {
            return;
        }

        if (StationedShips.ContainsKey(shipType))
        {
            StationedShips[shipType] += amount;
        }
        else
        {
            StationedShips.Add(shipType, amount);
        }
    }

    ///// <summary>
    ///// Remove ships from this planet
    ///// </summary>
    ///// For attacking? or moving ship out of planet?
    //public bool RemoveShips(ShipTypeSO shipType, int amount)
    //{
    //    if (shipType == null || !StationedShips.ContainsKey(shipType)|| StationedShips[shipType] < amount)
    //    {
    //        return false;
    //    }

    //    StationedShips[shipType] -= amount;
    //    if (StationedShips[shipType] <= 0)
    //    {
    //        StationedShips.Remove(shipType);
    //    }

    //    return true;
    //}

    ///// <summary>
    ///// FOR UI!
    ///// </summary>
    //public int GetShipCount(ShipTypeSO shipType)
    //{
    //    return StationedShips.ContainsKey(shipType) ? StationedShips[shipType] : 0;
    //}

    //// For attack and defense calculations, like Risk
    //public int GetTotalAssaultShips()
    //{
    //    int total = 0;
    //    foreach (var ship in StationedShips)
    //    {
    //        if (ship.Key.ActionType == ShipActionType.Combat)
    //        {
    //            total += ship.Value;
    //        }
    //    }
    //    return total;
    //}

    //public void StationShips(Dictionary<ShipTypeSO, int> stationShips)
    //{
    //    foreach (KeyValuePair<ShipTypeSO, int> kvp in stationShips)
    //    {
    //        if (this.StationedShips.ContainsKey(kvp.Key))
    //        {
    //            this.StationedShips[kvp.Key] += kvp.Value;
    //            continue;
    //        }
    //        this.StationedShips[kvp.Key] = kvp.Value;
    //    }
    //}

    //public void DebugPurposes()
    public void RaiseAffection(FactionType rizzler, int affection)
    {
        if (this.Affection.TryGetValue(rizzler, out int currAffection))
        {
            this.Affection[rizzler] = Math.Clamp(currAffection + affection, 0, 100);
        }
    }

    public void BuildStructure(StructureType structure)
    {
        if (this.Structures.Contains(structure)) return;
        this.Structures.Add(structure);
    }

   
}
