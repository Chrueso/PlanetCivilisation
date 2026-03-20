using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IEntityController, IDisposable
{
    private EntityModel model;
    private EntityView view;
    private MapGrid mapGrid;
    private CommandInvoker commandInvoker;
    private TurnManager turnManager;

    public bool IsCurrentTurn { get; private set; }

    private HashSet<GridHex> hexesInMoveRadius = new HashSet<GridHex>();

    private EventBinding<GameStartEvent> gameStartBinding;
    private EventBinding<TurnChangeEvent> turnChangeEventBinding;

    public PlayerController(EntityModel model, EntityView view)
    {
        this.model = model;
        this.view = view;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(HandleTurnChange);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        commandInvoker = gameStartEvent.CommandInvoker;
        turnManager = gameStartEvent.TurnManager;
        mapGrid = gameStartEvent.MapGrid;
        Debug.Log("Player recieved game context");

        ConnectModel();
        UpdateHexesInMoveRadius();
    }

    private void HandleTurnChange(TurnChangeEvent turnChangeEvent)
    {
        IsCurrentTurn = turnChangeEvent.CurrentTurnFaction == model.FactionType;
    }

    private void ConnectModel()
    {
        model.OnCurrentHexChanged += UpdateHexesInMoveRadius;
    }

    public FactionType GetFaction() => model.FactionType;

    public GridHex GetCurrentHex() => model.CurrentHex;

    public bool CheckIfHexIsInMoveRadius(GridHex hex) => hexesInMoveRadius.Contains(hex);

    public void UpdateHexesInMoveRadius()
    {
        if (mapGrid == null)
        {
            Debug.Log(this + "Map grid is null!");
            return;
        }

        hexesInMoveRadius.Clear();
        List<GridHex> list = mapGrid.Grid.GetGridObjectsInRadius(model.CurrentHex.GridPositionCube, model.MoveRadius);

        foreach (GridHex hex in list)
        {
            hexesInMoveRadius.Add(hex);
        }
    }

    public bool CanExecuteAction()
    {
        if (!IsCurrentTurn)
        {
            Debug.Log(this + "Not your turn!");
            return false;
        }

        if (commandInvoker == null) 
        {
            Debug.Log(this + "CommandInvoker is null!");
            return false;
        }

        return true;
    }

    public bool TryEndTurn()
    {
        if (!CanExecuteAction()) return false;

        turnManager.ChangeTurn(); 
        return true;
    }

    public bool TryMove(GridHex targetHex)
    {
        if (!CanExecuteAction()) return false;

        if (hexesInMoveRadius.Contains(targetHex))
        {
            ICommand command = new MoveCommand(model, view, targetHex);
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + "Outside move radius");
        return false;
    }

    public bool TryColonize(PlanetData planet)
    {
        if (!CanExecuteAction()) return false;

        if (planet.FactionType == FactionType.Nothing)
        {
            ICommand command = new ColonizeCommand(model, planet);
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log(this + "You cannot colonize a owned planet!");
        return false;
    }

    public bool TryAttack(PlanetData planet)
    {
        if (!CanExecuteAction()) return false;
        return true;
    }

    public bool TryBuildStructure(PlanetData planet, StructureType structure)
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
        model.OnCurrentHexChanged -= UpdateHexesInMoveRadius;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
        EventBus<TurnChangeEvent>.Deregister(turnChangeEventBinding);
    }
}
