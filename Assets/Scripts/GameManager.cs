using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }

    public System.Random SeedRNG;

    [Header("Settings")]
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private ShipDatabaseSO shipDatabase;

    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerInteractionController playerInteractionController;
    //[SerializeField] private DiplomacySystem diplomacySystem;

    [Header("Views")]
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private EntityView entityView;
    [SerializeField] private List<PlanetVisualTypesSO> planetVisualPresets;

    [Header("UI Views")]
    [SerializeField] private InfoMenuView infoMenuView;
    [SerializeField] private ActionsTabView actionsTabView;
    [SerializeField] private HUDView hudView;
    [SerializeField] private PlanetListView planetListView;
    [SerializeField] private StructuresMenuView structuresMenuView;
    [SerializeField] private SettingsView settingsView;

    // runtime
    private PlanetGenerator planetGenerator;
    private MapGenerator mapGenerator;
    private BattleManager battleManager;
    private CommandInvoker commandInvoker;
    private EntityFactory entityFactory;
    private InfoMenuController infoMenuController;
    private ActionsTabController actionsTabController;
    private HUDController hudController;
    private PlanetListController planetListController;
    private StructuresController structuresController;
    private SettingsController settingsController;

    //TO CHANGE
    //public DiplomacySystem DiplomacyInstance => diplomacySystem;
    public MapGrid MapGrid { get; private set; } // not used
    public Player Player { get; private set; } // not used

    //[SerializeField] private FactionManager factionManagerRef;

    private readonly List<IDisposable> disposables = new(); // for cleanup

    protected override void Awake()
    {
        base.Awake();

        CreateSystems();
        CreateHUD();
        CreateActionTab();

        // Game context
        GenerateSeed();

        mapGenerator.GenerateMap(mapSettings, out MapGrid mapGrid, out PlanetData homePlanet, SeedRNG);
        MapGrid = mapGrid;

        CreatePlayer(homePlanet, out EntityModel playerModel, out EntityView playerView, out PlayerController playerController);
        //Create ai here

        EventBus<GameStartEvent>.Raise(new GameStartEvent
        {
            CommandInvoker = commandInvoker,
            MapGrid = mapGrid,
            PlayerController = playerController,
            PlayerModel = playerModel,
            AIEntities = new List<EntityModel>()
        });
    }

    private void CreateSystems()
    {
        cameraController.Init();
        playerInteractionController.Init(cameraController);
        planetGenerator = new PlanetGenerator(planetVisualPresets, planetPrefab);
        mapGenerator = new MapGenerator(planetGenerator);
        //turnManager = new TurnManager(factionManagerRef);
        battleManager = new BattleManager(shipDatabase);
        commandInvoker = new CommandInvoker();
        entityFactory = new EntityFactory(entityView);

        TryRegisterDisposable(
            planetGenerator,
            mapGenerator,
            battleManager,
            commandInvoker,
            entityFactory
        );
    }

    private void CreateHUD()
    {
        settingsController = new SettingsController(settingsView); //maybe should be mono persistant from main menu
        planetListController = new PlanetListController(planetListView);
        hudController = new HUDController(hudView, cameraController, planetListController, settingsController);

        TryRegisterDisposable(settingsController, planetListController, hudController);
    }

    private void CreateActionTab()
    {
        infoMenuController = new InfoMenuController(infoMenuView);
        structuresController = new StructuresController(structuresMenuView);
        actionsTabController = new ActionsTabController(actionsTabView, playerInteractionController, infoMenuController, structuresController);

        TryRegisterDisposable(infoMenuController, structuresController, actionsTabController);
    }

    private void CreatePlayer(PlanetData homePlanet, out EntityModel playerModel, out EntityView playerView, out PlayerController playerController)
    {
        playerController = entityFactory.CreatePlayer(homePlanet, FactionType.Human, homePlanet.CurrentHex.WorldPosition,
           out playerModel, out playerView);

        TryRegisterDisposable(playerController);
    }

    private void CreateAI()
    {

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

    private void TryRegisterDisposable(params object[] objects)
    {
        foreach (object obj in objects)
        {
            if (obj is IDisposable disposable)
                disposables.Add(disposable);
        }
    }

    private void OnDestroy()
    {
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }
        disposables.Clear();
    }

}
