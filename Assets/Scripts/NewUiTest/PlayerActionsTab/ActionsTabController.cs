using System;

public class ActionsTabController : IUIMenuController
{
    private ActionsTabView view;
    private PlayerController playerController;
    private GridHex selectedHex;

    public static event Action OnViewClose;

    public ActionsTabController(ActionsTabView view, PlayerController playerController)
    {
        this.view = view;
        this.playerController = playerController;

        PlayerInteractionController.OnHexSelected += HandleHexSelected;
        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.MoveButton.onClick.AddListener(HandleMoveButtonClicked);
        view.AttackButton.onClick.AddListener(HandleAttackButtonClicked);
        view.ColonizeButton.onClick.AddListener(HandleColonizeButtonClicked);

        view.Init(playerController);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        OnViewClose?.Invoke();
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

    
    
}
