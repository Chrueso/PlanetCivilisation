using UnityEngine;

public class StructuresController : IUIMenuController
{
    private StructuresMenuView view;

    public StructuresController(StructuresMenuView view)
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
