using System;

public class ActionsTabController : IUIMenuController
{
    private ActionsTabView view;
    private InfoMenuController infoMenuController;
    private PlayerController playerController;
    private PlayerInteractionController playerInteractionController;
    private GridHex selectedHex;

    public ActionsTabController(ActionsTabView view, InfoMenuController infoMenuController, PlayerController playerController, PlayerInteractionController playerInteractionController)
    {
        this.view = view;

        this.infoMenuController = infoMenuController;
        this.playerController = playerController;
        this.playerInteractionController = playerInteractionController;

        playerInteractionController.OnHexSelected += HandleHexSelected;
        ConnectView();
    }

    public void ConnectView()
    {
        view.Init(playerController);

        view.CloseButton.onClick.AddListener(CloseView);
        view.InfoButton.onClick.AddListener(HandleInfoButtonClicked);
        view.MoveButton.onClick.AddListener(HandleMoveButtonClicked);
        view.ColonizeButton.onClick.AddListener(HandleColonizeButtonClicked);
        view.AttackButton.onClick.AddListener(HandleAttackButtonClicked);
        view.DiplomacyButton.onClick.AddListener(HandleDiplomacyButtonClicked);
        view.BuildStructureButton.onClick.AddListener(HandleBuildStructuresButtonClicked);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        playerInteractionController.UnselectHex();
    }

    public void HandleInfoButtonClicked()
    {
        if (selectedHex.Occupant != null)
        {
            infoMenuController.ShowInfo(selectedHex.Occupant);
        }
    }

    public void HandleHexSelected(GridHex selectedHex)
    {
        this.selectedHex = selectedHex;
        view.UpdateCurrentHex(selectedHex);
        GameScreenManager.Push(view);
    }

    private void HandleMoveButtonClicked()
    {
        if (playerController.TryMove(selectedHex))
        {
            CloseView();
        }
    }

    private void HandleColonizeButtonClicked()
    {
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

    }
    
    
}
