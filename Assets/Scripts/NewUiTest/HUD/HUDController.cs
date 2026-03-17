using System;
using UnityEngine;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;
    CameraController cameraController;

    public HUDController(HUDView view, EntityModel playerModel, CameraController cameraController) //Needs model when model resources update then this updates
    {
        this.view = view;
        this.playerModel = playerModel;
        this.cameraController = cameraController;

        ConnectView();
    }

    public void ConnectView()
    {
        view.SettingsButton.onClick.AddListener(HandleSettingsButtonClicked);
        view.PlanetListButton.onClick.AddListener(HandlePlanetListButtonClicked);
        view.HomeShipButton.onClick.AddListener(HandleHomeShipButtonClicked);
        view.EndTurnButton.onClick.AddListener(HandleEndTurnButtonClicked);

        playerModel.OnResourcesChanged += HandleResourcesChanged;

        view.UpdateFaction(playerModel.FactionType);
        view.HandleResources();
        view.UpdateResources(playerModel.Resources);
    }

    public void CloseView()
    {
       //Should u be able to close hud idk???
    }

    private void HandleSettingsButtonClicked()
    {
       
    }

    private void HandlePlanetListButtonClicked()
    {
        
    }

    private void HandleHomeShipButtonClicked()
    {
        cameraController.CenterToHomeShip(playerModel.CurrentHex.WorldPosition);
    }

    private void HandleEndTurnButtonClicked()
    {
        
    }

    private void HandleResourcesChanged()
    {
        view.UpdateResources(playerModel.Resources);
    }
}
