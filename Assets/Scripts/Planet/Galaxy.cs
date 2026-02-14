using System.Collections.Generic;
using UnityEngine;

public class Galaxy : Singleton<Galaxy> 
{
    [SerializeField] int planetAmount = 50;
    public string GalaxyName { get; private set; }
    public List<Planet> AllPlanets = new List<Planet>();

    public void Init(string galaxyName, System.Random rng)
    {
        GalaxyName = galaxyName;    
        List<Planet> planets = new List<Planet>();

        for (int i = 0; i < planetAmount; i++)
        {
            float x = RandomUtil.NextRangef(rng, -30, 30);
            float z = RandomUtil.NextRangef(rng, -30, 30); 
            float y = RandomUtil.NextRangef(rng, -10, 10);

            Vector3 position = new Vector3(x, y, z);
            Planet planet = PlanetGenerator.Instance.InstantiatePlanet(rng, position, this.transform);
            planets.Add(planet);
        }
    }

}
