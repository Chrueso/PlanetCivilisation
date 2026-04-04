using TMPro.Examples;
using UnityEngine;

public class PlanetListController : IUIMenuController
{
    private PlanetListView view;

    private CameraController cameraController;
    private EntityModel playerModel;

    public PlanetListController(PlanetListView view, CameraController cameraController)
    {
        this.view = view;
        this.cameraController = cameraController;


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
            view.InitalizeList(playerModel.OwnedPlanets, HandlePlanetClicked);
        }

        GameScreenManager.Push(view);
    }

    // Teleport to the planet's location on the map and close the menu
    private void HandlePlanetClicked(PlanetData planet)
    {
        if(planet.CurrentHex != null)
        {
            cameraController.MoveCamera(planet.CurrentHex.WorldPosition);
            CloseView();
        }
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }
}
