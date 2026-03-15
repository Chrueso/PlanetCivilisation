using UnityEngine;

public class PlayerActionController
{
    PlayerActionsView view;
    PlayerController playerController;
    GridHex gridHex;

    public PlayerActionController(PlayerActionsView view, PlayerController playerController)
    {
        this.view = view;
        this.playerController = playerController;
        
        ConnectView();

    }

    private void ConnectView()
    {
        view.attackButton.onClick.AddListener(OnAttackButtonClicked);
        view.colonizeButton.onClick.AddListener(OnColonizeButtonClicked);
        view.moveButton.onClick.AddListener(OnMoveButtonClicked);
    }
    

    private void ConnectModel()
    {
        //lets say u out of pp gray out the buttons maybe
    }

    private void OnAttackButtonClicked()
    {

    }

    private void OnColonizeButtonClicked()
    {

    }
    
    private void OnMoveButtonClicked()
    {
        ICommand command = new MoveCommand();
        CommandInvoker.ExecuteCommand(command);
    }
}
