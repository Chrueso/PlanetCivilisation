using System.Collections.Generic;
using UnityEngine;

public class PlanetMapGenerator
{
    private int radiusBetweenPlanets = 2;
    private int maxPlanets = 20;
    private CustomPlanetSO homePlanetData;
    private MapGrid mapGrid;
    private PlanetGenerator planetGenerator;
    private System.Random rng;

    public PlanetMapGenerator(int radiusBetweenPlanets, int maxPlanets, CustomPlanetSO homePlanetData, PlanetGenerator planetGenerator, System.Random rng)
    {
        this.radiusBetweenPlanets = radiusBetweenPlanets;
        this.maxPlanets = maxPlanets;
        this.homePlanetData = homePlanetData;
        this.planetGenerator = planetGenerator;
        this.rng = rng;
    }

    public void GeneratePlanets(out PlanetData homePlanet, Transform parent = default)
    {
        // For random index and fast deletion look at AvaliablePool.cs
        AvailablePool<GridHex> avaliableHexes = new AvailablePool<GridHex>();

        for (int z = 0; z < mapGrid.Grid.Height; z++)
        {
            for (int x = 0; x < mapGrid.Grid.Width; x++)
            {
                avaliableHexes.Add(mapGrid.Grid.GridArray[x, z]);
            }
        }

        bool hasSpawnedHomePlanet = false;
        homePlanet = null;

        // Choose hex and spawn planet
        int iter = 0;
        while (iter < maxPlanets && avaliableHexes.Count > 0)
        {
            GridHex hex = avaliableHexes.GetRandom(rng);

            // Spawn planet
            GameObject planetObject;
            PlanetData planetData;

            if (!hasSpawnedHomePlanet && this.homePlanetData != null)
            {
                (planetObject, planetData) = planetGenerator.GenerateCustomPlanet(this.homePlanetData, hex.WorldPosition, Quaternion.identity, parent);

                Debug.Log($"name {planetData.PlanetName} faction {planetData.FactionType}");

                planetData.CurrentHex = hex;
                homePlanet = planetData;
                hasSpawnedHomePlanet = true;
                
            }
            else
            {
                (planetObject, planetData) = planetGenerator.GeneratePlanet(rng, hex.WorldPosition, Quaternion.identity, parent);
                planetData.CurrentHex = hex;
            }

            planetObject.name = $"Planet{iter} {planetData.PlanetName}";

            // Assign planet to hex
            hex.Occupant = planetData;
            hex.IsOccupied = true;

            // Mark all hexes within a radius as unable to spawn
            List<GridHex> hexesToRemove = mapGrid.Grid.GetGridObjectsInRadius(hex.GridPositionCube, radiusBetweenPlanets);
            avaliableHexes.Remove(hexesToRemove);
            iter++;
        }
    }

    public void AssignFactionToPlanets(PlanetData homePlanet)
    {
        AvailablePool<GridHex> avaliableHexes = new AvailablePool<GridHex>();
        for (int z = 0; z < mapGrid.Grid.Height; z++)
        {
            for (int x = 0; x < mapGrid.Grid.Width; x++)
            {
                avaliableHexes.Add(mapGrid.Grid.GridArray[x, z]);
            }
        }

        // Choose random hex then check for planets in a radius around that hex if have planet assign faction to that planet then remove checked radius from avalible hex 
        // delegates 1 portion of grid to that faction
        int radiusBetweenFactions = mapGrid.Grid.Width / 2;
        var factionTypeValues = System.Enum.GetValues(typeof(FactionType));

        // Remove hexes in radius around homeplanet from avaliable hexes
        avaliableHexes.Remove(mapGrid.Grid.GetGridObjectsInRadius(homePlanet.CurrentHex.GridPositionCube, radiusBetweenFactions));

        // Set the remaining factions
        for (int i = 0; i < factionTypeValues.Length; i++)
        {
            FactionType factionType = (FactionType)factionTypeValues.GetValue(i);
            if (factionType != FactionType.Nothing && factionType != homePlanet.FactionType)
            {
                GridHex hex = avaliableHexes.GetRandom(rng);
                List<GridHex> hexesToCheck = mapGrid.Grid.GetGridObjectsInRadius(hex.GridPositionCube, radiusBetweenFactions);

                foreach (GridHex checkHex in hexesToCheck)
                {
                    // If hex has a planet
                    if (checkHex.Occupant is PlanetData planetData)
                    {
                        planetData.SetFaction(factionType);
                        //Debug.Log($"Planet: {planetData.PlanetName}, Faction: {planetData.FactionType}");
                    }

                }
            }
        }
    }

    private void DebugPlanet()
    {
        PlanetData pd = new("DebugPlanet", FactionType.Human);
    }
}
