using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> //CHANGE!
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random SeedRNG;

    [Header("Settings")]
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private ShipDatabaseSO shipDatabase;

    [Header("Prefabs")]
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private EntityView entityViewPrefab;
    [SerializeField] private List<PlanetVisualTypesSO> planetVisualPresets;

    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerInteractionController playerInteractionController;
    //[SerializeField] private DiplomacySystem diplomacySystem;

    [Header("UI")]
    [SerializeField] private ActionsTabView actionsTabView;

    // runtime
    private PlanetGenerator planetGenerator;
    private MapGenerator mapGenerator;
    private BattleManager battleManager;
    private CommandInvoker commandInvoker;
    private EntityFactory entityFactory;
    private ActionsTabController actionsTabController;

    //TO CHANGE
    //public DiplomacySystem DiplomacyInstance => diplomacySystem;
    public MapGrid MapGrid { get; private set; }
    public Player Player { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GenerateSeed();

        cameraController.Init();

        planetGenerator = new PlanetGenerator(planetVisualPresets, planetPrefab);
        mapGenerator = new MapGenerator(mapSettings, planetGenerator);
        mapGenerator.GenerateMap(out MapGrid mapGrid, out PlanetData homePlanet, SeedRNG); // MapGrid.GenerateGrid(50, 50, 6);
        MapGrid = mapGrid;

        battleManager = new BattleManager(shipDatabase);

        commandInvoker = new CommandInvoker();
        entityFactory = new EntityFactory(mapGrid, entityViewPrefab, commandInvoker);

        PlayerController playerController = entityFactory.CreatePlayer(homePlanet, FactionType.Human, homePlanet.CurrentHex.WorldPosition);

        //UI
        actionsTabController = new ActionsTabController(actionsTabView, playerController);

        playerInteractionController.Init(cameraController, mapGrid, actionsTabController);

        Vector3 homeplanetPos = homePlanet.CurrentHex.WorldPosition;
        Camera.main.transform.position = new Vector3(homeplanetPos.x, 55, homeplanetPos.z);


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
