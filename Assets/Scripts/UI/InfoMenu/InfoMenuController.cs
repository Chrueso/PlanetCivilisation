using UnityEngine;

public class InfoMenuController : IUIMenuController
{
    private InfoMenuView view;

    public InfoMenuController(InfoMenuView view)
    {
        this.view = view;

        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
    }

    public void OpenView()
    {
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }

    public void UpdateView(IGridHexObject hexOccupant)
    {
       
    }
}
