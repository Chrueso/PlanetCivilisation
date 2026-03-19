using System;
using UnityEngine;

public class HUDController : IUIMenuController, IDisposable
{
    private HUDView view;
    private EntityModel playerModel;
    private CameraController cameraController;
    private PlanetListController planetListController;
    private SettingsController settingsController;

    private EventBinding<GameStartEvent> gameStartBinding;

    public HUDController(HUDView view, CameraController cameraController, PlanetListController planetListController, SettingsController settingsController) 
    {
        this.view = view;

        this.cameraController = cameraController;
        this.planetListController = planetListController;
        this.settingsController = settingsController;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        ConnectView();
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        playerModel = gameStartEvent.PlayerModel;
        Debug.Log("HUD recieved player");
    }

    public void ConnectView()
    {
        playerModel.OnResourcesChanged += HandleResourcesChanged;

        view.UpdateFaction(playerModel.FactionType);
        view.HandleResources();
        view.UpdateResources(playerModel.Resources);

        view.SettingsButton.onClick.AddListener(HandleSettingsButtonClicked);
        view.PlanetListButton.onClick.AddListener(HandlePlanetListButtonClicked);
        view.HomeShipButton.onClick.AddListener(HandleHomeShipButtonClicked);
        view.EndTurnButton.onClick.AddListener(HandleEndTurnButtonClicked); 
    }

    public void OpenView()
    {
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }

    private void HandleResourcesChanged()
    {
        view.UpdateResources(playerModel.Resources);
    }

    private void HandleSettingsButtonClicked()
    {
       settingsController.OpenView();
    }

    private void HandlePlanetListButtonClicked()
    {
        planetListController.OpenView();
    }

    private void HandleHomeShipButtonClicked()
    {
        cameraController.MoveCamera(playerModel.CurrentHex.WorldPosition);
    }

    private void HandleEndTurnButtonClicked()
    {
        
    }

    public void Dispose()
    {
        playerModel.OnResourcesChanged -= HandleResourcesChanged;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }
}
