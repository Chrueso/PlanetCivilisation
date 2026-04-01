using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool isDebug = false;

    public string GalaxyName { get; private set; }
    public int SeedInt { get; private set; }
    public System.Random SeedRNG;

    [Header("Settings")]
    [SerializeField] private MapSettings mapSettings;
    [SerializeField] private ShipDatabaseSO shipDatabase;
    [SerializeField] private List<AIAction> aIActions; //I think factions have their own behavior later so store actions there?

    [Header("Mono Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GridInteractionController gridInteractionController;
    [SerializeField] private AudioSystem audioSystem;

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
    private TurnManager turnManager;
    private DiplomacySystem diplomacySystem;

    //Game context
    private MapGrid mapGrid;
    private PlanetData homePlanet;
    private HashSet<PlanetData> planets = new HashSet<PlanetData>();
    private IEntityController player;
    private Dictionary<AIBrain, IEntityController> AIEntities = new Dictionary<AIBrain, IEntityController>();

    private readonly List<IDisposable> disposables = new(); // for cleanup

    public void Awake()
    {
        CreateSystems();
        CreateHUD();
        CreateActionTab();

        GenerateSeed();

        mapGenerator.GenerateMap(mapSettings, out mapGrid, out homePlanet, out planets, SeedRNG);

        //CreatePlayer(homePlanet, out player);
        //CreateAI(FactionDatabase.factions.Length - 2, planets.ToList(), out AIEntities);

        //3 ai
        CreateAI(FactionDatabase.factions.Length - 1, planets.ToList(), out AIEntities);
        player = AIEntities.First().Value;
        AIEntities.Remove(AIEntities.First().Key);

        HashSet<IEntityController> aiControllers = new HashSet<IEntityController>();
        foreach (var kvp in AIEntities)
        {
            aiControllers.Add(kvp.Value);
        }

        //Give everything conntext to the game
        EventBus<GameStartEvent>.Raise(new GameStartEvent
        {
            MapGrid = mapGrid,
            PlayerController = player,
            AIControllers = aiControllers
        });

        //Start first turn
        turnManager.ChangeTurn();
    }

    private void CreateSystems()
    {
        cameraController.Init();
        gridInteractionController.Init(cameraController);
        planetGenerator = new PlanetGenerator(shipDatabase, planetVisualPresets, planetPrefab);
        mapGenerator = new MapGenerator(planetGenerator);
        turnManager = new TurnManager();
        battleManager = new BattleManager(shipDatabase);
        commandInvoker = new CommandInvoker();
        diplomacySystem = new();
        AudioService.SetAudioInstance(audioSystem);
        entityFactory = new EntityFactory(shipDatabase, entityView, commandInvoker, turnManager, battleManager, diplomacySystem);

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
        actionsTabController = new ActionsTabController(actionsTabView, gridInteractionController, infoMenuController, structuresController);

        TryRegisterDisposable(infoMenuController, structuresController, actionsTabController);
    }

    private void CreatePlayer(PlanetData homePlanet, out IEntityController player)
    {
        player = entityFactory.CreatePlayer(homePlanet, FactionType.Human);
        TryRegisterDisposable(player);
    }

    //This is ugly i want change later
    private void CreateAI(int amount, List<PlanetData> planets, out Dictionary<AIBrain, IEntityController> entities)
    {
        entities = new Dictionary<AIBrain, IEntityController>();
        if (planets == null || planets.Count == 0 || aIActions.Count == 0) return;

        List<PlanetData> availablePlanets = planets.FindAll(p => p.FactionType == FactionType.Nothing);
        if (availablePlanets.Count == 0) return;

        amount = Mathf.Min(amount, availablePlanets.Count);

        //Fisher yates
        for (int i = 0; i < amount; i++)
        {
            //shuffled to front?
            int j = UnityEngine.Random.Range(i, availablePlanets.Count);
            (availablePlanets[i], availablePlanets[j]) = (availablePlanets[j], availablePlanets[i]);
        }

        for (int i = 0; i < amount; i++)
        {
            EntityController entity = entityFactory.CreateAI(out AIBrain brain, availablePlanets[i], aIActions);
            if (entity != null)
            {
                entities.Add(brain, entity);
                TryRegisterDisposable(entity);
                TryRegisterDisposable(brain);
            }
        }
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

    private void OnValidate()
    {
        if (hudController == null) return;

        if (isDebug)
        {
            hudController.EnableDebug();
        }
        else
        {
            hudController.DisableDebug();
        }
    }

}
