using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ActionsTabController : IDisposable
{
    private ActionsTabView view;
    private IEntityController entityController;
    private GridInteractionController gridInteractionController;
    private StructuresMenuController structuresController;
    private InfoMenuView infoMenuView;
    private BuildMenuController buildMenuController;
    private StationShipMenuController stationShipMenuController;
    private TradeMenuController tradeMenuController;
    
    private GridHex selectedHex;
    private EntityScoutShipPool scoutShipPool;
// trade
    private List<EntityModel> allAIModels = new List<EntityModel>();
    private EventBinding<GameStartEvent> gameStartBinding;
    public ActionsTabController(ActionsTabView view, GridInteractionController gridInteractionController, StructuresMenuController structuresController, InfoMenuView infoMenuView, BuildMenuController buildMenuController, StationShipMenuController stationShipMenuController, EntityScoutShipPool scoutShipPool, TradeMenuController tradeMenuController)
    {
        this.view = view;

        this.gridInteractionController = gridInteractionController;
        this.structuresController = structuresController;
        this.scoutShipPool = scoutShipPool;
        this.infoMenuView = infoMenuView;
        this.buildMenuController = buildMenuController;
        this.stationShipMenuController = stationShipMenuController;
        this.tradeMenuController = tradeMenuController;

        gridInteractionController.OnHexSelected += HandleHexSelected;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
        
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        entityController = gameStartEvent.PlayerController;
        Debug.Log("ActionTabController recieved entity controller");
        

        allAIModels.Clear();
        foreach (var ai in gameStartEvent.AIControllers)
        {
            allAIModels.Add(ai.GetModel());
        }

        Debug.Log("ActionTabController recieved entity controller");
        ConnectView();
    }

    public void ConnectView()
    {
        if (entityController == null)
        {
            Debug.Log("ActionTab entityController is null!");
            return;
        }

        view.Init(entityController);

        view.CloseButton.onClick.AddListener(CloseView);
        view.InfoButton.onClick.AddListener(HandleInfoButtonClicked);
        view.MoveButton.onClick.AddListener(HandleMoveButtonClicked);
        view.MoveScoutButton.onClick.AddListener(HandleMoveScoutButtonClicked);
        view.ColonizeButton.onClick.AddListener(HandleColonizeButtonClicked);
        view.AttackButton.onClick.AddListener(HandleAttackButtonClicked);
        view.DiplomacyButton.onClick.AddListener(HandleDiplomacyButtonClicked);
        view.StructureButton.onClick.AddListener(HandleStructuresButtonClicked);
        view.BuildButton.onClick.AddListener(HandleBuildButtonClicked);
        view.StationShipsButton.onClick.AddListener(HandleStationShipsButtonClicked);
    }

    public void OpenView()
    {
        view.UpdateCurrentHex(selectedHex);
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        gridInteractionController.UnselectHex();
    }

    public void HandleHexSelected(GridHex h)
    {
        this.selectedHex = h;
        OpenView();
    }

    public void HandleInfoButtonClicked()
    {
        if (selectedHex.Occupant != null)
        {
            infoMenuView.UpdateInfo(selectedHex.Occupant);
            GameScreenManager.Push(infoMenuView);
        }
    }

    private void HandleMoveButtonClicked()
    {
        if (entityController != null && entityController.TryMove(selectedHex))
        {
            CloseView();
        }
    }

    private void HandleMoveScoutButtonClicked()
    {
        if (entityController == null) return;
        EntityScoutShipView ssInstance = scoutShipPool.GetScoutShipInstance();
        ssInstance.SetPos(entityController.GetView().transform.position);
        if (entityController.TryMoveScoutShip(selectedHex, ssInstance))
        {
            CloseView();
        }
        

    }

    private void HandleColonizeButtonClicked()
    {
        if (entityController == null) return;

        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            if (entityController.TryColonize(planet))
            {
                //CloseView();
                view.Show(true); // updates after colonize
            }
        }
    }

    private void HandleAttackButtonClicked()
    {
        if (entityController != null && selectedHex.Occupant is PlanetData planet)
            if (entityController.TryAttack(planet)) CloseView();
    }


    private void HandleDiplomacyButtonClicked()
    {
        if (entityController != null && selectedHex.Occupant is PlanetData planet)
            tradeMenuController.OpenView(entityController.GetModel(), planet);
    }

    private void HandleStructuresButtonClicked()
    {
        structuresController.OpenView(entityController);
    }

    private void HandleBuildButtonClicked()
    {
        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            // Only allow trading with AI controlled planets (Not player, not empty)
            if (planet.FactionType != FactionType.Nothing && planet.FactionType == entityController.GetModel().FactionType)
            {
                buildMenuController.OpenView(planet, entityController);
            }
            else
            {
                Debug.LogWarning("Cannot trade with an empty planet or your own planet!");
            }
        }
    }

    private void HandleStationShipsButtonClicked()
    {
        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet && entityController != null)
        {
            stationShipMenuController.OpenView(planet, entityController);
        }
    }

    public void Dispose()
    {
        if (this.selectedHex != null && this.selectedHex.Occupant != null)
            this.selectedHex.Occupant.OnDataChanged -= view.UpdateView;

        gridInteractionController.OnHexSelected -= HandleHexSelected;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }
}
