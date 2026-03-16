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
        GameScreenManager.Pop();
        this.selectedHex = selectedHex;
        view.UpdateCurrentHex(selectedHex);
        GameScreenManager.Push(view);
    }

    private void ConnectView()
    {
        view.CloseButton.onClick.AddListener(OnCloseButtonClicked);
        view.AttackButton.onClick.AddListener(OnAttackButtonClicked);
        view.ColonizeButton.onClick.AddListener(OnColonizeButtonClicked);
        view.MoveButton.onClick.AddListener(OnMoveButtonClicked);

        view.Init(playerController);
    }
    
    private void OnCloseButtonClicked()
    {
        GameScreenManager.Pop();
        OnViewClose?.Invoke();
    }

    private void OnAttackButtonClicked()
    {

    }

    private void OnColonizeButtonClicked()
    {

    }
    
    private void OnMoveButtonClicked()
    {
        
    }
}
