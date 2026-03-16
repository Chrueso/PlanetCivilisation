using UnityEngine;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;

    public HUDController(HUDView view, EntityModel playerModel) //Needs model when model resources update then this updates
    {
        this.view = view;
        this.playerModel = playerModel;
    }

    public void ConnectView()
    {
        //playerModel.OnResourcesChanged += HandleResourcesChanged();
        
    }

    public void CloseView()
    {
       //Should u be able to close hud idk???
    }
}
