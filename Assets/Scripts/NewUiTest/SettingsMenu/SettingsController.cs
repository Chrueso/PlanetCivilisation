using UnityEngine;

public class SettingsController : IUIMenuController
{
    private SettingsView view;

    public SettingsController(SettingsView view)
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
