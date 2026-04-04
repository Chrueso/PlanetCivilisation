using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : IEntityController, IDisposable
{
    //Components
    private EntityModel model;
    
    private EntityView view;
    private CommandInvoker commandInvoker;
    private TurnManager turnManager;
    private BattleManager battleManager;
    private DiplomacySystem diplomacySystem;
    private Crafter crafter;

    //gameconfig
    private GameConfigSO gameConfig;

    //Context
    public bool IsCurrentTurn { get; private set; }
    public event Action OnCurrentTurn;

    public bool IsPerformingAction;
    public event Action OnActionComplete;

    public MapGrid MapGrid { get; private set; }
    public HashSet<GridHex> HexesInMoveRadius { get; private set; } = new HashSet<GridHex>();
    private List<GetShipAfterTurnsPayload> shipBuildQueueList = new();

    private EventBinding<GameStartEvent> gameStartBinding;
    private EventBinding<TurnChangeEvent> turnChangeEventBinding;
    public bool IsActivePlayer { get; set; }

    public EntityController(EntityModel model, EntityView view, CommandInvoker commandInvoker, TurnManager turnManager, BattleManager battleManager, DiplomacySystem diplomacySystem, Crafter crafter, GameConfigSO gameConfig)
    {
        this.model = model;
        this.view = view;
        this.commandInvoker = commandInvoker;
        this.turnManager = turnManager;
        this.battleManager = battleManager;
        this.diplomacySystem = diplomacySystem;
        this.gameConfig = gameConfig;
        this.crafter = crafter;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(HandleTurnChange);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
        
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        MapGrid = gameStartEvent.MapGrid;
        Debug.Log("Entity recieved game context");

        ConnectModel();
        HandleCurrrentHexChanged();
    }

    private void HandleTurnChange(TurnChangeEvent turnChangeEvent)
    {
        IsCurrentTurn = turnChangeEvent.CurrentTurnFaction == model.FactionType;
        
        if (IsCurrentTurn)
        {
            model.CalculateResourceGain();
            model.RefreshAP();
            OnCurrentTurn?.Invoke();
        }

        if (shipBuildQueueList.Count > 0)
        {
            int currentTurnCount = turnChangeEvent.CurrentTurn;
            
            foreach (var shipBuild in shipBuildQueueList)
            {
                model.AddShips(shipBuild.ShipToBeBuilt, 1);
                shipBuild.DecrementAmount();
                if (shipBuild.ShipAmount <= 0)
                {
                    shipBuildQueueList.Remove(shipBuild);
                }
            }
        }
    }

    private void ConnectModel()
    {
        model.OnCurrentHexChanged += HandleCurrrentHexChanged;
    }

    public EntityModel GetModel() => model;
    public EntityView GetView() => view;

    public void HandleCurrrentHexChanged()
    {
        UpdateHexesInMoveRadius();
        UpdateDiscoveredHex();
    }

    public bool CheckIfHexIsInMoveRadius(GridHex hex) => HexesInMoveRadius.Contains(hex);

    public void UpdateHexesInMoveRadius()
    {
        if (MapGrid == null)
        {
            Debug.Log(this + " Map grid is null!");
            return;
        }

        HexesInMoveRadius.Clear();
        List<GridHex> list = MapGrid.Grid.GetGridObjectsInRadius(model.CurrentHex.GridPositionCube, model.MoveRadius);

        foreach (GridHex hex in list)
        {
            HexesInMoveRadius.Add(hex);
        }
    }

    public void UpdateDiscoveredHex()
    {
        foreach (GridHex hex in HexesInMoveRadius)
        {
            model.AddDiscoveredHex(hex);
            if (IsActivePlayer) hex.Show(); 
        }
    }

    public void UpdateVision()
    {
        foreach (GridHex hex in MapGrid.Grid.GridArray)
        {
            hex.Hide();
        }

        if (IsActivePlayer)
        {
            foreach (GridHex hex in model.DiscoveredHexes)
            {
                hex.Show();
            }
        }
    }

    public bool CanExecuteAction()
    {
        if (!IsCurrentTurn)
        {
            Debug.Log(this + " Not your turn!");
            return false;
        }

        if (IsPerformingAction)
        {
            Debug.Log(this + " Currently performing action");
            return false;
        }

        if (commandInvoker == null) 
        {
            Debug.Log(this + " CommandInvoker is null!");
            return false;
        }

        return true;
    }

    public bool HasAP()
    {
        if (model.CurrentAP <= 0)
        {
            Debug.Log(model.FactionType + " No AP");
            return false;
        }
        return true;
    }

    public bool TryEndTurn()
    {
        if (!CanExecuteAction()) return false;

        turnManager.ChangeTurn();
        model.CalculateResourceGain();
        return true;
    }

    public bool TryMove(GridHex targetHex)
    {
        if (!CanExecuteAction() || !HasAP()) return false;

        if (HexesInMoveRadius.Contains(targetHex))
        {
            ICommand command = new MoveCommand(this, model, view, targetHex, () => OnActionComplete?.Invoke());
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + " Outside move radius");
        return false;
    }

    public bool TryMoveScoutShip(GridHex targetHex, EntityScoutShipView scoutShipView)
    {
        if (!CanExecuteAction() || !HasAP()) return false;

        if (HexesInMoveRadius.Contains(targetHex))
        {
            ICommand command = new MoveScoutShipCommand(this, model, scoutShipView, targetHex, () => OnActionComplete?.Invoke());
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + " Outside move radius");
        return false;
    }

    public bool TryColonize(PlanetData planet)
    {
        if (!CanExecuteAction() || !HasAP()) return false;
        if (!model.EnoughShips(ShipType.Worker, gameConfig.MinWorkerShipNeededForColonize)/*added by chris*/) return false;
            if (planet.FactionType == FactionType.Nothing)
        {
            ICommand command = new ColonizeCommand(this, model, planet, () => OnActionComplete?.Invoke());
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + " You cannot colonize a owned planet!");
        return false;
    }

    public bool TryAttack(PlanetData planet)
    {
        if (!CanExecuteAction() || !HasAP()) return false;

        if (planet.FactionType != model.FactionType && planet.FactionType != FactionType.Nothing)
        {
            ICommand command = new AttackCommand(battleManager, this, model, planet, () => OnActionComplete?.Invoke());
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + " Cannot attack idk");
        return false;
    }

    public bool TryBuildStructure(PlanetData planet, StructureType structure)
    {
        if (!CanExecuteAction()) return false;
        return true;
    }

    public bool TryStationShip(PlanetData planet, ShipType shipType, int amount)
    {
        if (!CanExecuteAction()) return false;
        return true;
    }

    // Diplomacy
    public void Trade(PlanetData planet)
    {

    }

    public void Gift(PlanetData planet)
    {

    }

    public void Agreement(PlanetData planet)
    {

    }

    public void Dispose()
    {
        model.OnCurrentHexChanged -= HandleCurrrentHexChanged;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
        EventBus<TurnChangeEvent>.Deregister(turnChangeEventBinding);
    }
}
