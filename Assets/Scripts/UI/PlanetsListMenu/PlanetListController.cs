using UnityEngine;

public class PlanetListController : IUIMenuController
{
    private PlanetListView view;
    private EntityModel playerModel;

    public PlanetListController(PlanetListView view)
    {
        this.view = view;

        EventBus<GameStartEvent>.Register(new EventBinding<GameStartEvent>(HandleGameStart));

        ConnectView();
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        playerModel = gameStartEvent.PlayerController.GetModel();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.Init();
    }

    public void OpenView()
    {
        // Tell the view to populate the scroll list before showing it
        if (playerModel != null)
        {
            view.InitalizeList(playerModel.OwnedPlanets);
        }

        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }
}
