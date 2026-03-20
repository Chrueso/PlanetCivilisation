using System;
using UnityEngine;

public class HUDController : IUIMenuController, IDisposable
{
    private HUDView view;
    private IEntityController entityController;
    private EntityModel entityModel;
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
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        SetController(gameStartEvent.PlayerController);
        SetModel(gameStartEvent.PlayerModel);
        ConnectView();
    }

    public void SetController(IEntityController entityController)
    {
        this.entityController = entityController;
        Debug.Log("HUD recieved entity controller");
    }

    public void SetModel(EntityModel entityModel)
    {
        this.entityModel = entityModel;
        Debug.Log("HUD recieved entity model");
    }

    public void ConnectView()
    {
        if (entityModel == null)
        {
            Debug.Log("HUD entityModel is null!");
            return;
        }

        entityModel.OnResourcesChanged += HandleResourcesChanged;

        view.UpdateFaction(entityModel.FactionType);
        view.HandleResources();
        view.UpdateResources(entityModel.Resources);

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
        if (entityModel == null) return;
        view.UpdateResources(entityModel?.Resources);
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
        cameraController.MoveCamera(entityModel.CurrentHex.WorldPosition);
    }

    private void HandleEndTurnButtonClicked()
    {
        if (entityController == null) return;
        entityController.TryEndTurn();
    }

    public void Dispose()
    {
        EventBus<GameStartEvent>.Deregister(gameStartBinding);

        if (entityModel == null) return;
        entityModel.OnResourcesChanged -= HandleResourcesChanged;
    }
}
