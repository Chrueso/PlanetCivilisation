using System;
using UnityEngine;

public class ActionsTabController : IDisposable
{
    private ActionsTabView view;
    private IEntityController entityController;
    private GridInteractionController gridInteractionController;
    private StructuresMenuController structuresController;
    private InfoMenuView infoMenuView;
    private BuildMenuController buildMenuController;

    private GridHex selectedHex;

    private EventBinding<GameStartEvent> gameStartBinding;

    public ActionsTabController(ActionsTabView view, GridInteractionController gridInteractionController, StructuresMenuController structuresController, InfoMenuView infoMenuView, 
        BuildMenuController buildMenuController)
    {
        this.view = view;

        this.gridInteractionController = gridInteractionController;
        this.structuresController = structuresController;
        this.infoMenuView = infoMenuView;
        this.buildMenuController = buildMenuController;

        gridInteractionController.OnHexSelected += HandleHexSelected;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        entityController = gameStartEvent.PlayerController;
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
        view.ColonizeButton.onClick.AddListener(HandleColonizeButtonClicked);
        view.AttackButton.onClick.AddListener(HandleAttackButtonClicked);
        view.DiplomacyButton.onClick.AddListener(HandleDiplomacyButtonClicked);
        view.StructureButton.onClick.AddListener(HandleStructuresButtonClicked);
        view.BuildButton.onClick.AddListener(HandleBuildButtonClicked);
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

    public void HandleHexSelected(GridHex selectedHex)
    {
        this.selectedHex = selectedHex;
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
        if (entityController == null) return;

        if (entityController.TryMove(selectedHex))
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
        if (entityController == null) return; 

        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            if (entityController.TryAttack(planet))
            {
                CloseView();
            }
        }

    }

    private void HandleDiplomacyButtonClicked()
    {

    }

    private void HandleStructuresButtonClicked()
    {
        structuresController.OpenView(entityController);
    }

    private void HandleBuildButtonClicked()
    {
        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            buildMenuController.OpenView(planet, entityController);
        }
    }

    public void Dispose()
    {
        gridInteractionController.OnHexSelected -= HandleHexSelected;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }
    
}
