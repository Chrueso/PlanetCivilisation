using System;
using UnityEngine;

public class ActionsTabController : IUIMenuController, IDisposable
{
    private ActionsTabView view;
    private PlayerController playerController;
    private PlayerInteractionController playerInteractionController;
    private InfoMenuController infoMenuController;
    private StructuresController structuresController;
    private GridHex selectedHex;

    private EventBinding<GameStartEvent> gameStartBinding;

    public ActionsTabController(ActionsTabView view, PlayerInteractionController playerInteractionController,
        InfoMenuController infoMenuController, StructuresController structuresController)
    {
        this.view = view;

        this.playerInteractionController = playerInteractionController;
        this.infoMenuController = infoMenuController;
        this.structuresController = structuresController;

        playerInteractionController.OnHexSelected += HandleHexSelected;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        playerController = gameStartEvent.PlayerController;
        Debug.Log("ActionTabController recieved player");

        ConnectView();
    }

    public void ConnectView()
    {
        if (playerController == null) return;
        
        view.Init(playerController);

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
        GameScreenManager.Pop();
        playerInteractionController.UnselectHex();
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
        if (playerController == null) return;

        if (playerController.TryMove(selectedHex))
        {
            CloseView();
        }
    }

    private void HandleColonizeButtonClicked()
    {
        if (playerController == null) return;

        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            if (playerController.TryColonize(planet))
            {
                //CloseView();
                view.Show(true); // updates after colonize
            }
        }
    }

    private void HandleAttackButtonClicked()
    {
        if (playerController == null) return; 

        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            if (playerController.TryAttack(planet))
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
        playerInteractionController.OnHexSelected -= HandleHexSelected;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }
    
}
