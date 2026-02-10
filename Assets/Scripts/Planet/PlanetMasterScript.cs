using UnityEngine;
using System.Collections.Generic;
public class PlanetMasterScript : MonoBehaviour
{
    private static PlanetMasterScript instance;
    public static PlanetMasterScript main => instance;
    private Dictionary<string, Planet> planetDict;
    public Dictionary<string, Planet> Planets => planetDict;

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private float radius = 1.2f;
    [SerializeField] private int maxBalls = 20;

    [SerializeField] private Vector2 spawnRegion = new Vector3(8, 4);


    private void Awake()
    {
        planetDict = new Dictionary<string, Planet>();
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    private void Start()
    {
        GeneratePlanet();
    }
    private void GeneratePlanet()
    {
        PoissonDiscSampler pds = new(spawnRegion.x, spawnRegion.y, radius);
        Vector2 halfSpawnRegion = spawnRegion * 0.5f;
        int iter = 0;
        foreach (Vector2 sample in pds.Samples())
        {
            Vector2 spawnPos = sample - halfSpawnRegion;
            Vector3 pos = new Vector3(spawnPos.x, 0, spawnPos.y);
            Vector3 halfPos = pos * 0.5f;
            GameObject planetObject = Instantiate(planetPrefab, pos, Quaternion.identity);
            planetObject.name = $"Planet{iter}";
            Planet planetInfo = new(pos: pos, 1, 1);
            planetDict.Add(planetObject.name, planetInfo);
            iter++;
            if (iter >= maxBalls) break;
        }
    }

}
