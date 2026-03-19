using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IEntityController, IDisposable
{
    private EntityModel model;
    private EntityView view;
    private MapGrid mapGrid;
    private CommandInvoker commandInvoker;

    private HashSet<GridHex> hexesInMoveRadius = new HashSet<GridHex>();
    public HashSet<GridHex> HexesInMoveRadius => hexesInMoveRadius; 

    public GridHex CurrentHex => model.CurrentHex;
    public FactionType Faction => model.FactionType;

    private EventBinding<GameStartEvent> gameStartBinding;

    public PlayerController(EntityModel model, EntityView view)
    {
        this.model = model;
        this.view = view;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        ConnectModel();
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        commandInvoker = gameStartEvent.CommandInvoker;
        mapGrid = gameStartEvent.MapGrid;
        Debug.Log("Player recieved game context");

        UpdateHexesInMoveRadius();
    }

    private void ConnectModel()
    {
        model.OnCurrentHexChanged += UpdateHexesInMoveRadius;
    }

    private void UpdateHexesInMoveRadius()
    {
        if (mapGrid == null) return;

        hexesInMoveRadius.Clear();
        List<GridHex> list = mapGrid.Grid.GetGridObjectsInRadius(model.CurrentHex.GridPositionCube, model.MoveRadius);

        foreach (GridHex hex in list)
        {
            hexesInMoveRadius.Add(hex);
        }
    }

    public bool TryMove(GridHex targetHex)
    {
        if (commandInvoker == null) return false;

        if (hexesInMoveRadius.Contains(targetHex))
        {
            ICommand command = new MoveCommand(model, view, targetHex);
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log("Outside move radius");
        return false;
    }

    public bool TryColonize(PlanetData planet)
    {
        if (commandInvoker == null) return false;

        if (planet.FactionType == FactionType.Nothing)
        {
            ICommand command = new ColonizeCommand(model, planet);
            commandInvoker.ExecuteCommand(command);
            return true;
        }

        Debug.Log("You cannot colonize a owned planet!");
        return false;
    }

    public bool TryAttack(PlanetData planet)
    {
        if (commandInvoker == null) return false;
        return false;
    }

    public bool TryBuildStructure(PlanetData planet, StructureType structure)
    {
        if (commandInvoker == null) return false;
        return false;
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
    }
}
