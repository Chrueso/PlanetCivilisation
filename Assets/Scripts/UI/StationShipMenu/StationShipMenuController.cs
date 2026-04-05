using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class StationShipMenuController
{
    private StationShipMenuView view;
    private ShipDatabaseSO shipDatabase;
    private PlanetData planet;
    private IEntityController entityController;

    public StationShipMenuController(StationShipMenuView view, ShipDatabaseSO shipDatabase)
    {
        this.view = view;
        this.shipDatabase = shipDatabase;

        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.Init(shipDatabase.Ships.Values, this.planet, HandleStationShipElementClicked);
    }

    public void OpenView(PlanetData planet, IEntityController entityController)
    {
        this.planet = planet;
        this.entityController = entityController;
        view.UpdateView(planet);
        GameScreenManager.Push(view);
    }

    private void HandleStationShipElementClicked(ShipType shipType)
    {
        entityController.TryStationShip(planet, shipType, 1);
        view.UpdateView(planet);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        view.ClearPreviousElements();
    }
}
