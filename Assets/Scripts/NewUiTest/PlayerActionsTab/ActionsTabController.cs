using UnityEngine;
using System;

public class ActionsTabController
{
    private ActionsTabView view;
    private PlayerController playerController;
    private GridHex selectedHex;
    public event Action OnViewClose;

    public ActionsTabController(ActionsTabView view, PlayerController playerController)
    {
        this.view = view;
        this.playerController = playerController;

        ConnectView();
    }

    public void HandleHexSelected(GridHex selectedHex)
    {
        this.selectedHex = selectedHex;
        view.UpdateCurrentHex(selectedHex);
        GameScreenManager.Push(view);
    }

    private void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.MoveButton.onClick.AddListener(OnMoveButtonClicked);
        view.AttackButton.onClick.AddListener(OnAttackButtonClicked);
        view.ColonizeButton.onClick.AddListener(OnColonizeButtonClicked);

        view.Init(playerController);
    }

    private void CloseView()
    {
        GameScreenManager.Pop();
        OnViewClose?.Invoke();
    }

    private void OnMoveButtonClicked()
    {
        if (playerController.TryMove(selectedHex))
        {
            CloseView();
        }
    }

    private void OnColonizeButtonClicked()
    {
        if (selectedHex.Occupant != null && selectedHex.Occupant is PlanetData planet)
        {
            if (playerController.TryColonize(planet))
            {
                CloseView();
            }
        }
    }

    private void OnAttackButtonClicked()
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
