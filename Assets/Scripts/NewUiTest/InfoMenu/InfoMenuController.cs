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

    public void OpenView()
    {
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }

    public void UpdateView(IGridHexOccupant hexOccupant)
    {
       
    }
}
