using System;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.ObjectSpin;
using static UnityEngine.Rendering.DebugUI;
public class PlanetManager : Singleton<PlanetManager> 
{
    public Dictionary<string, PlanetData> PlanetDict { get; private set; } = new Dictionary<string, PlanetData>();

    [SerializeField] private int radiusBetweenPlanets = 2;
    [SerializeField] private int maxPlanets = 20;
    [SerializeField] private CustomPlanetSO homePlanetData;

    public void GeneratePlanets(MapGrid mapGrid, System.Random rng, out PlanetData homePlanet)
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

            if (!hasSpawnedHomePlanet && homePlanetData != null)
            {
                (planetObject, planetData) = PlanetGenerator.Instance.GenerateCustomPlanet(homePlanetData, hex.WorldPosition, Quaternion.identity, this.transform);
                print($"name {planetData.PlanetName} faction {planetData.FactionType}");
                planetData.CurrentHex = hex;
                homePlanet = planetData;
                hasSpawnedHomePlanet = true;
                
            }
            else
            {
                (planetObject, planetData) = PlanetGenerator.Instance.GeneratePlanet(rng, hex.WorldPosition, Quaternion.identity, this.transform);
                planetData.CurrentHex = hex;
            }

            planetObject.name = $"Planet{iter} {planetData.PlanetName}";
            PlanetDict.Add(planetObject.name, planetData);

            // Assign planet to hex
            hex.Occupant = planetData;
            hex.IsOccupied = true;

            // Mark all hexes within a radius as unable to spawn
            List<GridHex> hexesToRemove = mapGrid.Grid.GetGridObjectsInRadius(hex.GridPositionCube, radiusBetweenPlanets);
            avaliableHexes.Remove(hexesToRemove);
            iter++;
        }
    }

    public void AssignFactionToPlanets(MapGrid mapGrid, System.Random rng, PlanetData homePlanet)
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
                        Debug.Log(planetData.PlanetName);
                    }

                }
            }
        }
    }

    private void DebugPlanet()
    {
        PlanetData pd = new("DebugPlanet", FactionType.Human);
        PlanetDict.Add(pd.PlanetName, pd);
    }

    //public void GeneratePlanets(System.Random rng)
    //{
    //    PoissonDiscSampler pds = new(spawnRegion.x, spawnRegion.y, radiusBetweenPlanets);
    //    Vector2 halfSpawnRegion = spawnRegion * 0.5f;
    //    int iter = 0;

    //    foreach (Vector2 sample in pds.Samples())
    //    {
    //        Vector2 spawnPos = sample - halfSpawnRegion;
    //        Vector3 pos = new Vector3(spawnPos.x, 20, spawnPos.y);
    //        Vector3 halfPos = pos * 0.5f;
            
    //        (var planetObject, var planetData) = PlanetGenerator.Instance.GeneratePlanet(rng, pos, Quaternion.identity, this.transform);

    //        planetObject.name = $"Planet{iter}";

    //        PlanetDict.Add(planetObject.name, planetData);
    //        iter++;
    //        if (iter >= maxPlanets) break;
    //    }
    //}
}
