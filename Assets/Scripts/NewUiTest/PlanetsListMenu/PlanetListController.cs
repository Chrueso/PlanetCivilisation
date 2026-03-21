using UnityEngine;

public class PlanetListController : IUIMenuController
{
    private PlanetListView view;

    public PlanetListController(PlanetListView view)
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
}
