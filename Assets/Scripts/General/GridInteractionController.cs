using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GridInteractionController : MonoBehaviour
{
    private CameraController cameraController;
    private Camera cam;
    private MapGrid mapGrid;
    private GridHex selectedHex;

    private bool touchStartedOnUI = false;

    public event Action<GridHex> OnHexSelected;
    private EventBinding<GameStartEvent> gameStartBinding;

    public void Init(CameraController cameraController)
    {
        TouchscreenHandler.FingerDownCallback += OnFingerDown;
        TouchscreenHandler.FingerUpCallback += OnSelectGrid;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        this.cameraController = cameraController;
        cam = cameraController.CameraInstance;
    }

    private void OnDisable()
    {
        TouchscreenHandler.FingerDownCallback -= OnFingerDown;
        TouchscreenHandler.FingerUpCallback -= OnSelectGrid;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        SetMap(gameStartEvent.MapGrid);
    }

    public void SetMap(MapGrid mapGrid)
    {
        this.mapGrid = mapGrid;
        Debug.Log("GridInteractionController received game map");
    }

    public void OnFingerDown(object sender, TouchInfo touchInfo)
    {
        touchStartedOnUI = EventSystem.current.IsPointerOverGameObject(touchInfo.Current.touchId);
    }

    private void OnSelectGrid(object sender, TouchInfo touchInfo)
    {
        if (mapGrid == null)
        {
            Debug.Log(this + "Map grid is null!");
            return;
        }
        if (cameraController.CameraMoving) return;
        if (touchStartedOnUI) return;

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

    public void UnselectHex()
    {
        if (selectedHex != null)
        {
            selectedHex.GridHexVisual.OnSelected();
            selectedHex = null;
        }
    }
}

