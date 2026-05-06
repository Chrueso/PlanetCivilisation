using UnityEngine;

public class PlayerInfoController
{
    private PlayerInfoView view;
    private TurnManager turnManager; 
    public PlayerInfoController(PlayerInfoView playerInfoView, TurnManager turnManager)
    {
        this.view = playerInfoView;
        this.turnManager = turnManager;
    }
    public void OpenView(EntityModel entityModel)
    {
        // Tell the view to populate the scroll list before showing it
        if (entityModel != null)
        {
            view.SetPlayer(entityModel, turnManager);
        }

        GameScreenManager.Push(view);
    }
}
