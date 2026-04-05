using UnityEngine;

public class StationShipMenuController
{
    private StationShipMenuView view;
    private ShipDatabaseSO shipDatabase;
    private PlanetData currentPlanetData;
    private EntityModel currentEntityModel; // Track the player's model
    private IEntityController entityController;

    public StationShipMenuController(StationShipMenuView view, ShipDatabaseSO shipDatabase)
    {
        this.view = view;
        this.shipDatabase = shipDatabase;

        ConnectView();
    }

    public void ConnectView()
    {
        view.closeButton.onClick.AddListener(CloseView);
    }

    // Pass the EntityModel (player) in along with the planet
    public void OpenView(PlanetData planetData, IEntityController entityController)
    {
        if (planetData == null || shipDatabase == null || shipDatabase.Ships == null || entityController.GetModel() == null) return;

        currentPlanetData = planetData;
        currentEntityModel = entityController.GetModel();
        this.entityController = entityController;

        // Pass the raw data and the callback function downward
        view.Populate(shipDatabase.Ships.Values, currentPlanetData, TryBuildShip);

        GameScreenManager.Push(view);
    }

    // The Controller handles ALL data modification and logic
    private void TryBuildShip(ShipDataSO shipData)
    {
        if (currentPlanetData == null || shipData == null || currentEntityModel == null) return;
        if (!entityController.TryBuildShip(currentPlanetData, shipData.Type, 1)) return;
        view.RefreshCounts(currentPlanetData);
        // 1. Validate: Check if the player has enough resources in their global inventory
        /*
        foreach (var req in shipData.RequiredResources)
        {
            if (!currentEntityModel.Resources.TryGetValue(req.ResourceType, out int amount))
            {
                Debug.LogWarning($"Player does not have {req.ResourceType} to build {shipData.Type}.");
                return;
            }

            if (amount < req.Amount)
            {
                Debug.LogWarning($"Not enough {req.ResourceType} to build {shipData.Type}. Need {req.Amount}, have {amount}.");
                return;
            }
        }

        // 2. Transaction: Consume resources from the PLAYER
        foreach (var req in shipData.RequiredResources)
        {
            currentEntityModel.TakeResource(req.ResourceType, req.Amount);
            Debug.Log($"Consumed {req.Amount} of {req.ResourceType} from player to build {shipData.Type}.");
        }

        // 3. Execution: Build/Add ship to the planet
        currentPlanetData.AddShips(shipData.Type, 1);
        */
        // 4. Update the View

    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        currentPlanetData = null;
        currentEntityModel = null;
        view.ClearPreviousElements();
    }
}
