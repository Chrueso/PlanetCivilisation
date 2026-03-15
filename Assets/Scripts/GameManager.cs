using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> //CHANGE!
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random SeedRNG;

    private PlanetGenerator planetGenerator;
    private MapGenerator mapGenerator;
    private BattleManager battleManager;
    private CommandInvoker commandInvoker;

    [Header("Map")]
    [SerializeField] private MapSettings mapSettings;
    // addd map asset for prefabs i think
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private List<PlanetVisualTypesSO> planetVisualPresets;

    [Header("Player")]
    [SerializeField] private Player playerPrefab;
    [SerializeField] private ShipDatabaseSO shipDatabase;

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;

    [SerializeField] private DiplomacySystem diplomacySystem;
    public DiplomacySystem DiplomacyInstance => diplomacySystem;

    public MapGrid MapGrid { get; private set; }
    public Player Player { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GenerateSeed();

        planetGenerator = new PlanetGenerator(planetVisualPresets, planetPrefab);
        mapGenerator = new MapGenerator(mapSettings, planetGenerator);
        mapGenerator.GenerateMap(out MapGrid mapGrid, out PlanetData homePlanet, SeedRNG); // MapGrid.GenerateGrid(50, 50, 6);
        MapGrid = mapGrid;

        battleManager = new BattleManager(shipDatabase);

        Player = Instantiate(playerPrefab);
        Player.Init(homePlanet, FactionType.Human);

        Vector3 homeplanetPos = homePlanet.CurrentHex.WorldPosition;
        Camera.main.transform.position =  new Vector3(homeplanetPos.x, 55, homeplanetPos.z);
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
