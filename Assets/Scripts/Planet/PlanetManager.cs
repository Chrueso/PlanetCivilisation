using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager> 
{
    public Dictionary<string, PlanetData> PlanetDict { get; private set; } = new Dictionary<string, PlanetData>();

    [SerializeField] private int radiusBetweenPlanets = 2;
    [SerializeField] private int maxPlanets = 20;
    //[SerializeField] private Vector2 spawnRegion = new Vector3(8, 4);

    public void GeneratePlanets(MapGrid mapGrid, System.Random rng)
    {
        // For random index and fast deletion
        List<GridHex> avaliableHexes = new List<GridHex>();
        Dictionary<GridHex, int> avaliableHexesLookup = new Dictionary<GridHex, int>();
        int index = 0;
        for (int z = 0; z < mapGrid.Grid.Height; z++)
        {
            for (int x = 0; x < mapGrid.Grid.Width; x++)
            {
                avaliableHexes.Add(mapGrid.Grid.GridArray[x, z]);
                avaliableHexesLookup.Add(mapGrid.Grid.GridArray[x, z], index);
                index++;
            }
        }

        // Choose hex and spawn planet
        int iter = 0;
        while (iter < maxPlanets && avaliableHexes.Count > 0)
        {
            int randomHexIndex = rng.Next(0, avaliableHexes.Count);

            GridHex hex = avaliableHexes[randomHexIndex];

            // Spawn planet
            (var planetObject, var planetData) = PlanetGenerator.Instance.GeneratePlanet(rng, hex.WorldPosition, Quaternion.identity, this.transform);
            planetObject.name = $"Planet{iter}";
            planetData.HexOccupying = hex;
            PlanetDict.Add(planetObject.name, planetData);

            // Assign planet to hex
            hex.Occupant = planetData;
            hex.IsOccupied = true;

            // Mark all hexes within a radius as unable to spawn
            List<GridHex> hexesToRemove = mapGrid.Grid.GetGridObjectsInRadius(hex.GridPositionCube, radiusBetweenPlanets);
            foreach (GridHex hexToRemove in hexesToRemove)
            {
                if (avaliableHexesLookup.TryGetValue(hexToRemove, out int removeIndex))
                {
                    int lastIndex = avaliableHexes.Count - 1;
                    GridHex lastHex = avaliableHexes[lastIndex];

                    // Swap the hex to remove with the last one
                    avaliableHexes[removeIndex] = lastHex;

                    // Update the dictionary for the moved hex
                    avaliableHexesLookup[lastHex] = removeIndex;

                    // Remove the last element
                    avaliableHexes.RemoveAt(lastIndex);
                    avaliableHexesLookup.Remove(hexToRemove);

                    // Do this so u dont shift the stored incides
                }
            }
            iter++;
        }
        DebugPlanet();
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
