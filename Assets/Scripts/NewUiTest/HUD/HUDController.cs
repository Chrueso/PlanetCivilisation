using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;

    public HUDController(HUDView view, EntityModel playerModel) //Needs model when model resources update then this updates
    {
        this.view = view;
        this.playerModel = playerModel;

        ConnectView();
    }

    public void ConnectView()
    {
        view.SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        view.PlanetListButton.onClick.AddListener(OnPlanetListButtonClicked);
        view.HomeShipButton.onClick.AddListener(OnHomeShipButtonClicked);
        view.EndTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

        playerModel.OnResourcesChanged += HandleResourcesChanged;

        view.UpdateFaction(playerModel.FactionType);
    }

    public void CloseView()
    {
       //Should u be able to close hud idk???
    }

    private void OnSettingsButtonClicked()
    {
        // open settings view or maybe raise event then settings controller + view open
    }

    private void OnPlanetListButtonClicked()
    {
        // same thing another planet list controller + view probably unless low logic
    }

    private void OnHomeShipButtonClicked()
    {

    }

    private void OnEndTurnButtonClicked()
    {
        // tells turn manager through event 
    }

    private void HandleResourcesChanged()
    {
        view.UpdateResources(playerModel.Resources);
    }
}
