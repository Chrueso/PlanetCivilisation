using System.Collections.Generic;

public class PlanetData : IGridHexOccupant
{
    public string PlanetName { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>();
    public Dictionary<ShipTypeSO,int> Ships { get; private set; } = new Dictionary<ShipTypeSO, int>();
    public FactionType FactionType { get; private set; }
    public int Structures { get; private set; }

    public PlanetData(string planetName, List<ResourceType> resourceTypes, FactionType factionType)
    {
        PlanetName = planetName;

        // Assign initial resources
        Resources.Add(ResourceType.Rock, 0); // every planet has rock by default not sure if still the case?

        // Assign the randomly generated resource type
        foreach (var resource in resourceTypes)
        {
            Resources.Add(resource, 0);
        }

        FactionType = factionType;
    }

    /// <summary>
    /// Add ships to this planet. Works for Scout, Assault, Worker, or any ShipTypeSO
    /// </summary>
    // For chris to make ship crafting i guess
    public void AddShips(ShipTypeSO shipType, int amount)
    {
        if (shipType == null || amount <= 0)
        {
            return;
        }

        if (Ships.ContainsKey(shipType))
        {
            Ships[shipType] += amount;
        }
        else
        {
            Ships.Add(shipType, amount);
        }
    }

    /// <summary>
    /// Remove ships from this planet
    /// </summary>
    /// For attacking? or moving ship out of planet?
    public bool RemoveShips(ShipTypeSO shipType, int amount)
    {
        if (shipType == null || !Ships.ContainsKey(shipType)|| Ships[shipType] < amount)
        {
            return false;
        }

        Ships[shipType] -= amount;
        if (Ships[shipType] <= 0)
        {
            Ships.Remove(shipType);
        }

        return true;
    }

    /// <summary>
    /// FOR UI!
    /// </summary>
    public int GetShipCount(ShipTypeSO shipType)
    {
        return Ships.ContainsKey(shipType) ? Ships[shipType] : 0;
    }

    // For attack and defense calculations, like Risk
    public int GetTotalAssaultShips()
    {
        int total = 0;
        foreach (var ship in Ships)
        {
            if (ship.Key.ActionType == ShipActionType.Combat)
            {
                total += ship.Value;
            }
        }
        return total;
    }

    public void DebugPurposes()
    {
        Structures++;
    }

    //Idk what is this for yet so i comment -Zen
    //private void IncreaseResource()
    //{
    //    if (Resources.Count > 0)
    //    {
    //        foreach (var resource in Resources.Keys)
    //        {
    //            Resources[resource] += 1;
    //        }
    //    }
    //}

    //public void GainResourcePerTurn() 
    //{
    //    // Add resource to player data
    //}

    //public void GainStructureBenefit()
    //{
    //    // Based on structure call functions to stuff
    //}

    //private void IncreasePopulation()
    //{

    //}

    //private void GetRarerOre()
    //{

    //}

    //private void CreateShip()
    //{

    //}
}
