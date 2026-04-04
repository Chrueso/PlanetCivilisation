using UnityEngine;

public class SettingsController : MonoBehaviour
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
