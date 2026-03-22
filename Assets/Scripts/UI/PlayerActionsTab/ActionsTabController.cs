using System;
using UnityEngine;

public class ActionsTabController : IUIMenuController, IDisposable
{
    private ActionsTabView view;
    private IEntityController entityController;
    private GridInteractionController gridInteractionController;
    private InfoMenuController infoMenuController;
    private StructuresController structuresController;
    private GridHex selectedHex;

    private EventBinding<GameStartEvent> gameStartBinding;

    public ActionsTabController(ActionsTabView view, GridInteractionController gridInteractionController,
        InfoMenuController infoMenuController, StructuresController structuresController)
    {
        this.view = view;

        this.gridInteractionController = gridInteractionController;
        this.infoMenuController = infoMenuController;
        this.structuresController = structuresController;

        gridInteractionController.OnHexSelected += HandleHexSelected;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        SetController(gameStartEvent.PlayerController);
        ConnectView();
    }

    public void SetController(IEntityController entityController)
    {
        this.entityController = entityController;
        Debug.Log("ActionTabController recieved entity controller");
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
        view.BuildStructureButton.onClick.AddListener(HandleBuildStructuresButtonClicked);
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
            infoMenuController.UpdateView(selectedHex.Occupant);
            infoMenuController.OpenView();
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

    private void HandleBuildStructuresButtonClicked()
    {
        structuresController.OpenView();
    }
    
    public void Dispose()
    {
        gridInteractionController.OnHexSelected -= HandleHexSelected;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }
    
}
