using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IEntityController
{
    private EntityModel model;
    private EntityView view;
    private MapGrid mapGrid;
    private CommandInvoker commandInvoker;

    private HashSet<GridHex> hexesInMoveRadius = new HashSet<GridHex>();
    public HashSet<GridHex> HexesInMoveRadius => hexesInMoveRadius; 

    public GridHex CurrentHex => model.CurrentHex;
    public FactionType Faction => model.FactionType;

    public PlayerController(EntityModel model, EntityView view, MapGrid mapGrid, CommandInvoker commandInvoker)
    {
        this.model = model;
        this.view = view;
        this.mapGrid = mapGrid;
        this.commandInvoker = commandInvoker;
        ConnectModel();
    }

    private void ConnectModel()
    {
        model.OnCurrentHexChanged += UpdateHexesInMoveRadius;
    }

    private void UpdateHexesInMoveRadius()
    {
        hexesInMoveRadius.Clear();
        List<GridHex> list = mapGrid.Grid.GetGridObjectsInRadius(model.CurrentHex.GridPositionCube, model.MoveRadius);

        foreach (GridHex hex in list)
        {
            hexesInMoveRadius.Add(hex);
        }
    }

    public bool TryMove(GridHex targetHex)
    {
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
        return false;
    }

    public bool TryBuildStructure(PlanetData planet, StructureType structure)
    {
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
}
