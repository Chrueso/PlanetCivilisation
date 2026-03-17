using UnityEngine;

public class InfoMenuController : IUIMenuController
{
    private InfoMenuView view;

    public InfoMenuController(InfoMenuView view)
    {
        this.view = view;
    }

    public void ConnectView()
    {
       
    }

    public void CloseView()
    {
        
    }

    public void ShowInfo(IGridHexOccupant hexOccupant)
    {

    }
}
