using System;
using UnityEngine;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;
    private PlanetListController planetListController;
    private CameraController cameraController;

    public HUDController(HUDView view, EntityModel playerModel, PlanetListController planetListController, CameraController cameraController) 
    {
        this.view = view;
        this.playerModel = playerModel;
        this.planetListController = planetListController;
        this.cameraController = cameraController;

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
