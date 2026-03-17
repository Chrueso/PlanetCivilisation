using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerInteractionController : MonoBehaviour
{
    private CameraController cameraController;
    private Camera cam;
    private MapGrid mapGrid;
    private GridHex selectedHex;

    public static event Action<GridHex> OnHexSelected;

    private void OnDisable()
    {
        TouchscreenHandler.FingerUpCallback -= OnSelectGrid;
        ActionsTabController.OnViewClose -= UnselectHex;
    }

    public void Init(CameraController cameraController, MapGrid mapGrid)
    {
        this.cameraController = cameraController;
        cam = cameraController.CameraInstance;
        this.mapGrid = mapGrid;

        TouchscreenHandler.FingerUpCallback += OnSelectGrid;
        ActionsTabController.OnViewClose += UnselectHex;
    }

    private void OnSelectGrid(object sender, TouchInfo touchInfo)
    {
        if (EventSystem.current.IsPointerOverGameObject(touchInfo.Current.touchId)) return;

        if (mapGrid == null) return;
        if (cameraController.CameraMoving) return;

        UnselectHex();

        Ray ray = cam.ScreenPointToRay(touchInfo.ScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridHex hex = mapGrid.Grid.GetGridObject(hit.point);

            if (hex != null)
            {
                selectedHex = hex;
                selectedHex.GridHexVisual.OnSelected();

                OnHexSelected?.Invoke(selectedHex);

                //cameraInstance.transform.position = new(grid.WorldPosition.x, 55, grid.WorldPosition.z);
                //PlayerCam.Disable();
            }
            else
            {
                GameScreenManager.Pop();
            }

        }
    }

    private void UnselectHex()
    {
        if (selectedHex != null)
        {
            selectedHex.GridHexVisual.OnSelected();
            selectedHex = null;
        }
    }
}

