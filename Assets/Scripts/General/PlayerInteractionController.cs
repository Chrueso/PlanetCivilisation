using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerInteractionController : MonoBehaviour
{
    private CameraController cameraController;
    private Camera cam;
    private MapGrid mapGrid;
    private GridHex selectedHex;

    public event Action<GridHex> OnHexSelected;

    private EventBinding<GameStartEvent> gameStartBinding;

    private void OnEnable()
    {
        TouchscreenHandler.FingerUpCallback += OnSelectGrid;

        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
    }

    private void OnDisable()
    {
        TouchscreenHandler.FingerUpCallback -= OnSelectGrid;
        EventBus<GameStartEvent>.Deregister(gameStartBinding);
    }

    public void Init(CameraController cameraController)
    {
        this.cameraController = cameraController;
        cam = cameraController.CameraInstance;
    }

    public void HandleGameStart(GameStartEvent gameStartEvent)
    {
        mapGrid = gameStartEvent.MapGrid;
        Debug.Log("PlayerInteractionController received game context");
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

    public void UnselectHex()
    {
        if (selectedHex != null)
        {
            selectedHex.GridHexVisual.OnSelected();
            selectedHex = null;
        }
    }
}

