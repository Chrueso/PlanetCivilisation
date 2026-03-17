using System;

public class HUDController : IUIMenuController
{
    private HUDView view;
    private EntityModel playerModel;

    public static event Action OnSettingsButtonsClicked;
    public static event Action OnPlanetListButtonClicked;
    public static event Action OnHomeShipButtonClicked;
    public static event Action OnEndTurnButtonClicked;

    public HUDController(HUDView view, EntityModel playerModel) //Needs model when model resources update then this updates
    {
        this.view = view;
        this.playerModel = playerModel;

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
    }

    public void CloseView()
    {
       //Should u be able to close hud idk???
    }

    private void HandleSettingsButtonClicked()
    {
        OnSettingsButtonsClicked?.Invoke();
    }

    private void HandlePlanetListButtonClicked()
    {
        OnPlanetListButtonClicked?.Invoke();
    }

    private void HandleHomeShipButtonClicked()
    {
        OnHomeShipButtonClicked?.Invoke();
    }

    private void HandleEndTurnButtonClicked()
    {
        OnEndTurnButtonClicked?.Invoke();
    }

    private void HandleResourcesChanged()
    {
        view.UpdateResources(playerModel.Resources);
    }
}
