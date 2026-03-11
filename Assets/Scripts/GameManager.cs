using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    [SerializeField] MapSettings mapSettings;
    [SerializeField] private Player playerPrefab;

    public System.Random SeedRNG;

    PlanetGenerator planetGenerator;
    MapGenerator mapGenerator;
    BattleManager battleManager;

    [SerializeField] private List<PlanetVisualTypesSO> planetVisualPresets;
    [SerializeField] private GameObject planetPrefab;

    public MapGrid MapGrid { get; private set; }
    public Player Player { get; private set; }

    [SerializeField] private ShipDatabaseSO ShipDatabaseSO;

    protected override void Awake()
    {
        base.Awake();

        GenerateSeed();

        planetGenerator = new PlanetGenerator(planetVisualPresets, planetPrefab);
        mapGenerator = new MapGenerator(mapSettings, planetGenerator);
        mapGenerator.GenerateMap(out MapGrid mapGrid, out PlanetData homePlanet, SeedRNG); // MapGrid.GenerateGrid(50, 50, 6);
        MapGrid = mapGrid;

        battleManager = new BattleManager(ShipDatabaseSO);

        Player = Instantiate(playerPrefab);
        Player.Init(homePlanet, FactionType.Human);

        Vector3 homeplanetPos = homePlanet.CurrentHex.WorldPosition;
        CameraController.Instance.CameraInstance.transform.position = new Vector3(homeplanetPos.x, CameraController.Instance.CameraInstance.transform.position.y, homeplanetPos.z);
    }

    private void GenerateSeed()
    {
        // First generate galaxy name which is the seed for everything
        GalaxyName = GalaxyGenerator.GenerateGalaxyName();
        SeedInt = SeedUtil.StringToHashCode(GalaxyName);

        // Then using the galaxy seed to generate seeds for the things in game ask me if need clarity -Zen
        // MoonRNG etc...
        // Basically anything that needs a seed for generation in game should use this pattern
        //MapRNG = new System.Random(SeedInt);
        //PlanetRNG = new System.Random(SeedInt + 1000);
        // not needed

        SeedRNG = new System.Random(SeedInt);

        Debug.Log($"Generated Galaxy Name: {GalaxyName} with Seed: {SeedInt}");
    }

}
