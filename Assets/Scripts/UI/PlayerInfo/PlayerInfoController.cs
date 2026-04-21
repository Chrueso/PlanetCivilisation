using UnityEngine;

public class PlayerInfoController
{
    private PlayerInfoView view;

    public PlayerInfoController(PlayerInfoView playerInfoView)
    {
        this.view = playerInfoView;
    }
    public void OpenView(EntityModel entityModel)
    {
        // Tell the view to populate the scroll list before showing it
        if (entityModel != null)
        {
            view.SetPlayer(entityModel);
        }

        GameScreenManager.Push(view);
    }
}
