using System;

public class ActionsTabController : IUIMenuController
{
    private ActionsTabView view;
    private PlayerController playerController;
    private PlayerInteractionController playerInteractionController;
    private InfoMenuController infoMenuController;
    private StructuresController structuresController;
    private GridHex selectedHex;

    public ActionsTabController(ActionsTabView view, PlayerController playerController, PlayerInteractionController playerInteractionController,
        InfoMenuController infoMenuController, StructuresController structuresController)
    {
        this.view = view;

        this.playerController = playerController;
        this.playerInteractionController = playerInteractionController;
        this.infoMenuController = infoMenuController;
        this.structuresController = structuresController;

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
        structuresController.OpenView();
    }
    
    
}
