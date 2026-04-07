using DG.Tweening;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool orthographicPanning = false;

    // can hardcode for now since map size is fixed
    private Vector2 minBounds = Vector2.zero;
    private Vector2 maxBounds = new Vector2(512, 512);

    private bool eventsEnabled = true;
    private bool waitForReset = false;
    private bool playerIsPinching = false;   
    public bool CameraMoving { get; private set; }
    public Camera CameraInstance { get; private set; }
    private Vector3 startingPos = Vector2.zero;
    public Vector3 CurrPos { get; private set; }
    private float z = 0f;

    private bool touchStartedOnUI = false;

    private EventBinding<GameStartEvent> gameStartBinding;

    // for pinch zoom cam :D
    private Dictionary<int, TouchInfo> trackedGestures = new();
    private bool isDirty = false;
    private Vector3 CurrentScreenPos1 = Vector3.zero;
    private Vector3 CurrentScreenPos2 = Vector3.zero;
    private Vector3 LastScreenPos1 = Vector3.zero;
    private Vector3 LastScreenPos2 = Vector3.zero;

    public void Init()
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);

        CameraInstance = Camera.main;
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);
    }

    private void OnEnable()
    {
        #region ORTHO EVENTS
        // 90 degree orthographic implementation
        if (orthographicPanning)
        {
            TouchscreenHandler.FingerDownCallback += PlayerFingerDown;
            TouchscreenHandler.FingerMoveCallback += PlayerFingerMove;
            TouchscreenHandler.FingerUpCallback += PlayerFingerRelease;
            return;
        }
        #endregion

        TouchscreenHandler.FingerDownCallback += OnPlayerFingerDown;
        TouchscreenHandler.FingerMoveCallback += OnPlayerFingerMove;
        TouchscreenHandler.FingerUpCallback += OnPlayerFingerRelease;
       
    }

    private void OnDisable()
    {
        #region ORTHO EVENTS
        if (orthographicPanning)
        {
            TouchscreenHandler.FingerDownCallback -= PlayerFingerDown;
            TouchscreenHandler.FingerMoveCallback -= PlayerFingerMove;
            TouchscreenHandler.FingerUpCallback -= PlayerFingerRelease;
            return;
        }
        #endregion

        TouchscreenHandler.FingerDownCallback -= OnPlayerFingerDown;
        TouchscreenHandler.FingerMoveCallback -= OnPlayerFingerMove;
        TouchscreenHandler.FingerUpCallback -= OnPlayerFingerRelease;

        EventBus<GameStartEvent>.Deregister(gameStartBinding);

    }
    #region PERSPECTIVE
    private void OnPlayerFingerRelease(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = GetWorldPos(z, e.ScreenPos);
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);
        
        touchStartedOnUI = EventSystem.current.IsPointerOverGameObject(e.Current.touchId);

    }

    private void OnPlayerFingerMove(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        if (waitForReset) return;
        if (touchStartedOnUI) return;   
        if (e.Current.delta.sqrMagnitude < 0.01f)
        {
            print("U KINDA SLOW?");
        }
        Vector3 direction = startingPos - GetWorldPos(z, e.ScreenPos);
        direction.y = 0f;
        //cameraInstance.transform.position += direction;
        Vector3 nextPos = CameraInstance.transform.position + direction;
        Vector3 boundedPos = new Vector3(Mathf.Clamp(nextPos.x, minBounds.x, maxBounds.x), nextPos.y, Mathf.Clamp(nextPos.z, minBounds.y, maxBounds.y));
        CameraInstance.transform.position = boundedPos;
    }

    private Vector3 GetWorldPos(float z, Vector3 pos)
    {
        Ray fingerPos = CameraInstance.ScreenPointToRay(pos);
        Plane plane = new Plane(CameraInstance.transform.forward, new Vector3(0,0,z));
        plane.Raycast(fingerPos, out float distance);
        return fingerPos.GetPoint(distance);

    }

    private void OnPlayerFingerDown(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = GetWorldPos(z, e.ScreenPos);
        if (waitForReset) waitForReset = false;
        
        touchStartedOnUI = EventSystem.current.IsPointerOverGameObject(e.Current.touchId);
    }
    #endregion

    #region ORTHO CAM
    private void PlayerFingerDown(object sender, TouchInfo e)
    {
        if (!eventsEnabled) return;
        
        if (e.FingerId == 1)
        {
            trackedGestures[e.FingerId] = e;
            CurrentScreenPos1 = LastScreenPos1 = e.ScreenPos;
        } else if (e.FingerId == 2)
        {
            trackedGestures[e.FingerId] = e;
            CurrentScreenPos2 = LastScreenPos2 = e.ScreenPos;
            playerIsPinching = true;
        }
        
        startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);
        if (waitForReset) waitForReset = false;
        touchStartedOnUI = EventSystem.current.IsPointerOverGameObject(e.Current.touchId);
    }

    private void PlayerFingerMove(object sender, TouchInfo e)
    {
        if (!eventsEnabled) return;
        if (waitForReset) return;
        if (touchStartedOnUI) return;

        if (!playerIsPinching)
        {
            Vector3 diff = CameraInstance.ScreenToWorldPoint(e.ScreenPos) - CameraInstance.transform.position;
            Vector3 nextPos = startingPos - diff;
            Vector3 boundedPos = new Vector3(Mathf.Clamp(nextPos.x, minBounds.x, maxBounds.x), 55, Mathf.Clamp(nextPos.z, minBounds.y, maxBounds.y));
            CameraInstance.transform.position = boundedPos;
            
            if (e.Phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);

            }
            else
            {
                CameraMoving = true;
            }
        } else
        {
            if (e.FingerId == 1)
            {
                LastScreenPos1 = CurrentScreenPos1;
                CurrentScreenPos1 = e.ScreenPos;
            }
            else if (e.FingerId == 2)
            {
                LastScreenPos2 = CurrentScreenPos2;
                CurrentScreenPos2 = e.ScreenPos;
            }
        }
    }

    private void PlayerFingerRelease(object sender, TouchInfo e)
    {
        if (!eventsEnabled) return;
        if (e.FingerId == 1)
        {
            LastScreenPos1 = LastScreenPos2;
            CurrentScreenPos1 = CurrentScreenPos2;
            trackedGestures.Remove(e.FingerId); 
            playerIsPinching = false;
        }
        else if (e.FingerId == 2)
        {
            LastScreenPos2 = Vector3.zero;
            CurrentScreenPos2 = Vector3.zero;
            trackedGestures.Remove(e.FingerId);
            playerIsPinching = false;
        }
        startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);
        CameraMoving = false;
        touchStartedOnUI = EventSystem.current.IsPointerOverGameObject(e.Current.touchId);
    }
    #endregion

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        MoveCamera(gameStartEvent.PlayerController.GetModel().CurrentHex.WorldPosition);
    }

    public void MoveCamera(Vector3 position)
    {
        Camera.main.transform.DOMove(new Vector3(position.x, Camera.main.transform.position.y, position.z),0.3f).SetEase(Ease.OutQuad);
    }

    private void DisableMovement()
    {
        if (!eventsEnabled) return;
        eventsEnabled = false;
        waitForReset = true;
    }

    private void EnableMovement()
    {
        if (eventsEnabled) return;

        eventsEnabled = true;
    }

    private void Update()
    {
        if (playerIsPinching)
        {
            float scale = 1f;
            float avgCurrDist = 0f;
            float avgLastDist = 0f;
            Vector2 currCenter = (CurrentScreenPos1 + CurrentScreenPos2) / 2f;
            Vector2 lastCenter = (LastScreenPos1 + LastScreenPos2) / 2f;
            float currDist = Vector2.Distance(CurrentScreenPos1, currCenter) + Vector2.Distance(CurrentScreenPos2, currCenter);
            float lastDist = Vector2.Distance(LastScreenPos1, lastCenter) + Vector2.Distance(LastScreenPos2, lastCenter);
            avgCurrDist += currDist;
            avgLastDist += lastDist;
            avgCurrDist /= 2f;
            avgLastDist /= 2f;
            if (avgLastDist > 0.0f)
            {
                scale = avgCurrDist / avgLastDist;
            }
            else
            {
                scale = 1.0f;
            }
            CameraInstance.orthographicSize = Mathf.Clamp(CameraInstance.orthographicSize * scale, 30, 100);
        }
        isDirty = false;
    }

    public void Disable() => DisableMovement();
    public void Enable() => EnableMovement();
}
