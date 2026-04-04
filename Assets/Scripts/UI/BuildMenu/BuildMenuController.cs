using UnityEngine;

public class BuildMenuController : IUIMenuController
{
    private BuildMenuView view;

    public BuildMenuController(BuildMenuView view, EntityController entityController)
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

    public void HandleBuildElementClicked(StructureType structureType)
    {

    }
}
