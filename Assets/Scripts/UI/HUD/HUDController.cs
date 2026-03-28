using System.Collections.Generic;
using System;
using UnityEngine;

public class HUDController : IUIMenuController, IDisposable
{
    private HUDView view;
    private IEntityController entityController;
    private IEntityController playerController;
    private EntityModel entityModel;
    private CameraController cameraController;
    private PlanetListController planetListController;
    private SettingsController settingsController;

    private EventBinding<GameStartEvent> gameStartBinding;

    //Debug
    private int currentEntityIndex = 0;
    private List<IEntityController> allEntities;

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
        entityController = gameStartEvent.PlayerController;
        playerController = gameStartEvent.PlayerController;
        entityModel = entityController.GetModel();
        Debug.Log("HUD recieved entity controller");

        ConnectView();

        //Debug setup
        allEntities = new List<IEntityController>();
        allEntities.Add(gameStartEvent.PlayerController);
        foreach (var ai in gameStartEvent.AIControllers)
        {
            allEntities.Add(ai);
        }
    }

    public void ConnectView()
    {
        if (entityModel == null)
        {
            Debug.Log("HUD entityModel is null!");
            return;
        }

        entityModel.OnResourcesChanged += HandleResourcesChanged;
        entityModel.OnAPChanged += HandleAPChanged;

        view.UpdateFaction(entityModel.FactionType);
        view.HandleResources();
        view.UpdateResources(entityModel.Resources);
        view.UpdateAP(entityModel.CurrentAP, entityModel.MaxAP);

        view.SettingsButton.onClick.AddListener(HandleSettingsButtonClicked);
        view.PlanetListButton.onClick.AddListener(HandlePlanetListButtonClicked);
        view.HomeShipButton.onClick.AddListener(HandleHomeShipButtonClicked);
        view.EndTurnButton.onClick.AddListener(HandleEndTurnButtonClicked);

        DisableDebug();
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
        view.UpdateResources(entityModel.Resources);
    }

    private void HandleAPChanged()
    {
        if (entityModel == null) return;
        view.UpdateAP(entityModel.CurrentAP, entityModel.MaxAP);
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
        //if (entityController == null) return;
        //entityController.TryEndTurn();

        if (playerController == null) return;
        playerController.TryEndTurn();
    }

    public void Dispose()
    {
        EventBus<GameStartEvent>.Deregister(gameStartBinding);

        if (entityModel == null) return;
        entityModel.OnResourcesChanged -= HandleResourcesChanged;
        entityModel.OnAPChanged -= HandleAPChanged;
    }

    //Debug
    public void EnableDebug()
    {
        view.EnableDebug();
        view.NextEntityButton.onClick.AddListener(HandleNextEntityButtonClicked);
    }

    public void DisableDebug()
    {
        view.NextEntityButton.onClick.RemoveListener(HandleNextEntityButtonClicked);
        view.DisableDebug();
    }

    public void HandleNextEntityButtonClicked()
    {
        currentEntityIndex = (currentEntityIndex + 1) % allEntities.Count;

        // Unsub the old model
        entityModel.OnResourcesChanged -= HandleResourcesChanged;

        // Update model
        entityController = allEntities[currentEntityIndex];
        entityModel = entityController.GetModel();

        entityModel.OnResourcesChanged += HandleResourcesChanged;
        view.UpdateFaction(entityModel.FactionType);
        view.UpdateResources(entityModel.Resources);
        view.UpdateAP(entityModel.CurrentAP, entityModel.MaxAP);

        cameraController.MoveCamera(entityModel.CurrentHex.WorldPosition);
      
        Debug.Log("HUD Displaying " + entityController.GetFaction());
    }
}
