using UnityEngine;
using System.Collections.Generic;

public class PlanetManager : Singleton<PlanetManager>
{
    public Dictionary<string, PlanetData> PlanetDict { get; private set; } = new Dictionary<string, PlanetData>();

    [SerializeField] private float radiusBetweenPlanets = 1.2f;
    [SerializeField] private int maxPlanets = 20;
    [SerializeField] private Vector2 spawnRegion = new Vector3(8, 4);

    public void GeneratePlanets(System.Random rng)
    {
        PoissonDiscSampler pds = new(spawnRegion.x, spawnRegion.y, radiusBetweenPlanets);
        Vector2 halfSpawnRegion = spawnRegion * 0.5f;
        int iter = 0;

        foreach (Vector2 sample in pds.Samples())
        {
            Vector2 spawnPos = sample - halfSpawnRegion;
            Vector3 pos = new Vector3(spawnPos.x, 20, spawnPos.y);
            Vector3 halfPos = pos * 0.5f;
            
            (var planetObject, var planetData) = PlanetGenerator.Instance.GeneratePlanet(rng, pos, Quaternion.identity, this.transform);

            planetObject.name = $"Planet{iter}";

            PlanetDict.Add(planetObject.name, planetData);
            iter++;
            if (iter >= maxPlanets) break;
        }
        DebugPlanet();
    }

    private void DebugPlanet()
    {
        PlanetData pd = new("DebugPlanet", new List<ResourceType>(), FactionType.Human);
        PlanetDict.Add(pd.PlanetName, pd);
    }
}
