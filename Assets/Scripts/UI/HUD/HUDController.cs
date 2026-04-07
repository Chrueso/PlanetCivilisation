using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HUDController : IDisposable
{
    private HUDView view;
    private IEntityController entityController;
    private IEntityController playerController;
    private EntityModel entityModel;
    private CameraController cameraController;
    private PlanetListController planetListController;
    private SettingsController settingsController;
    private TradeMenuController tradeMenuController;

    private EventBinding<GameStartEvent> gameStartBinding;

    //Debug
    private int currentEntityIndex = 0;
    private List<IEntityController> allEntities;

    // Store AI Models specifically for trading
    private List<EntityModel> allAIModels = new List<EntityModel>();

    public HUDController(HUDView view, CameraController cameraController, PlanetListController planetListController, SettingsController settingsController, TradeMenuController tradeMenuController) 
    {
        this.view = view;

        this.cameraController = cameraController;
        this.planetListController = planetListController;
        this.settingsController = settingsController;
        this.tradeMenuController = tradeMenuController;

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
        
        allAIModels.Clear();
        foreach (var ai in gameStartEvent.AIControllers)
        {
            allEntities.Add(ai);
            allAIModels.Add(ai.GetModel()); // Gather EntityModels specifically for Trade Menu
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
        view.TradeButton.onClick.AddListener(HandleTradeButtonClicked);

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
        planetListController.OpenView(entityModel);
    }

    private void HandleHomeShipButtonClicked()
    {
        cameraController.MoveCamera(entityModel.CurrentHex.WorldPosition);
    }

    private void HandleEndTurnButtonClicked()
    {
        if (playerController == null) return;
        playerController.TryEndTurn();
    }

    private void HandleTradeButtonClicked() 
    {
        if (tradeMenuController != null)
        {
            tradeMenuController.OpenView(playerController.GetModel(), allAIModels); 
        }
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
        entityModel.OnAPChanged -= HandleAPChanged;

        // Update model
        entityController = allEntities[currentEntityIndex];
        entityModel = entityController.GetModel();
        EventBus<HUDEntityChangeEvent>.Raise(new HUDEntityChangeEvent { NewEntity = entityController});

        entityModel.OnResourcesChanged += HandleResourcesChanged;
        entityModel.OnAPChanged += HandleAPChanged;

        view.UpdateFaction(entityModel.FactionType);
        view.UpdateResources(entityModel.Resources);
        view.UpdateAP(entityModel.CurrentAP, entityModel.MaxAP);

        cameraController.MoveCamera(entityModel.CurrentHex.WorldPosition);

        entityController.UpdateVision();

        Debug.Log("HUD Displaying " + entityModel.FactionType);
    }
}
