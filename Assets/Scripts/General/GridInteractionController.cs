using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInteractionController : MonoBehaviour
{
    [SerializeField] private GridHexSelectionView selectionViewPrefab;
    private CameraController cameraController;
    private Camera cam;
    private MapGrid mapGrid;
    private GridHex selectedHex;
    private EntityModel player;

    private bool touchStartedOnUI = false;

    private GridHexSelectionView selectionView;
    Tween selectionTween;

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
        this.mapGrid = gameStartEvent.MapGrid;
        this.player = gameStartEvent.PlayerController.GetModel();

        Debug.Log("GridInteractionController received game map");
        CreateSelectionHexView();
    }

    private void CreateSelectionHexView()
    {
        if (mapGrid == null)
        {
            Debug.Log(this + "Failed to create selection view map grid is null");
            return;
        }
        selectionView = Instantiate(selectionViewPrefab, this.transform);
        selectionView.Init(mapGrid.CellSize, mapGrid.HexView.OutlineThickness);
        selectionView.gameObject.SetActive(false);
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

        Ray ray = cam.ScreenPointToRay(touchInfo.ScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridHex hex = mapGrid.Grid.GetGridObject(hit.point);

            if (hex != null)
            {
                if (hex == selectedHex)
                {
                    UnselectHex(); // same hex toggle off
                    return;
                }
 
                UnselectHex(); // diff hex swap
                selectedHex = hex;
                ShowSelectionView();
                OnHexSelected?.Invoke(selectedHex);

                //Debug.Log(HexGridXZ<GridHex>.Distance(player.CurrentHex.GridPositionCube, hex.GridPositionCube)); //show distance from current hex
            }
            else
            {
                UnselectHex();
            }

        }
    }

    public void ShowSelectionView()
    {
        if (selectedHex != null && selectionView != null)
        {
            Vector3 pos = selectedHex.WorldPosition;
            pos.y = selectedHex.WorldPosition.y + 0.1f;
            selectionView.transform.position = pos;
            selectionView.gameObject.SetActive(true);

            selectionTween?.Kill(true);
            selectionView.transform.localScale = Vector3.one;
            selectionTween = selectionView.transform
                .DOPunchScale(Vector3.one * 0.2f, 0.25f);
        }
    }

    public void HideSelectionView()
    {
        if (selectionView != null)
        {
            selectionTween?.Kill(true);
            selectionView.transform.localScale = Vector3.one;
            selectionView.gameObject.SetActive(false);
        }
    }

    public void UnselectHex()
    {
        if (selectedHex != null)
        {
            HideSelectionView();
            selectedHex = null;
            GameScreenManager.Pop();

        }
    }
}

