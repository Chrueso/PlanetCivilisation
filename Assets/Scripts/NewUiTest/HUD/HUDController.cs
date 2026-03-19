using System;
using UnityEngine;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;
    private CameraController cameraController;
    private PlanetListController planetListController;
    private SettingsController settingsController;

    public HUDController(HUDView view, EntityModel playerModel, CameraController cameraController, PlanetListController planetListController, SettingsController settingsController) 
    {
        this.view = view;
        this.playerModel = playerModel;
        this.cameraController = cameraController;
        this.planetListController = planetListController;
        this.settingsController = settingsController;

        ConnectView();
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
        cameraController.CenterToHomeShip(playerModel.CurrentHex.WorldPosition);
    }

    private void HandleEndTurnButtonClicked()
    {
        
    }
}
