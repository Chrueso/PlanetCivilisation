using TMPro.Examples;
using UnityEngine;

public class PlanetListController : MonoBehaviour
{
    private PlanetListView view;

    private CameraController cameraController;

    public PlanetListController(PlanetListView view, CameraController cameraController)
    {
        this.view = view;
        this.cameraController = cameraController;

        ConnectView();
    }

    public void ConnectView()
    {
        view.CloseButton.onClick.AddListener(CloseView);
        view.Init();
    }

    public void OpenView(EntityModel entityModel)
    {
        // Tell the view to populate the scroll list before showing it
        if (entityModel != null)
        {
            view.InitalizeList(entityModel.OwnedPlanets, HandlePlanetClicked);
        }

        GameScreenManager.Push(view);
    }

    // Teleport to the planet's location on the map and close the menu
    private void HandlePlanetClicked(PlanetData planet)
    {
        if(planet.CurrentHex != null)
        {
            cameraController.MoveCamera(planet.CurrentHex.WorldPosition);
            CloseView();
        }
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
    }
}
