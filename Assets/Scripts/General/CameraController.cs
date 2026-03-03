using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : Singleton<CameraController> 
{
    [SerializeField] private bool orthographicPanning = false;

    // can hardcode for now since map size is fixed
    private Vector2 minBounds = Vector2.zero;
    private Vector2 maxBounds = new Vector2(512, 512);

    private bool eventsEnabled = true;
    private bool waitForReset = false;
    public Camera CameraInstance { get; private set; }
    private Vector3 startingPos = Vector2.zero;
    public Vector3 CurrPos { get; private set; }
    private float z = 0f;

    protected override void Awake()
    {
        base.Awake();
        CameraInstance = Camera.main;
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);
    }


    private void Start()
    {
        #region ORTHO EVENTS
        // 90 degree orthographic implementation
        if (orthographicPanning)
        {
            TouchscreenHandler.Instance.FingerDownCallback += PlayerFingerDown;
            TouchscreenHandler.Instance.FingerMoveCallback += PlayerFingerMove;
            TouchscreenHandler.Instance.FingerUpCallback += PlayerFingerRelease;
            return;
        }
        #endregion

        TouchscreenHandler.Instance.FingerDownCallback += OnPlayerFingerDown;
        TouchscreenHandler.Instance.FingerMoveCallback += OnPlayerFingerMove;
        TouchscreenHandler.Instance.FingerUpCallback += OnPlayerFingerRelease;
    }


    private void OnDisable()
    {
        #region ORTHO EVENTS
        if (orthographicPanning)
        {
            TouchscreenHandler.Instance.FingerDownCallback -= PlayerFingerDown;
            TouchscreenHandler.Instance.FingerMoveCallback -= PlayerFingerMove;
            TouchscreenHandler.Instance.FingerUpCallback -= PlayerFingerRelease;
            return;
        }
        #endregion

        TouchscreenHandler.Instance.FingerDownCallback -= OnPlayerFingerDown;
        TouchscreenHandler.Instance.FingerMoveCallback -= OnPlayerFingerMove;
        TouchscreenHandler.Instance.FingerUpCallback -= OnPlayerFingerRelease;

    }

    private void OnPlayerFingerRelease(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = GetWorldPos(z, e.ScreenPos);
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);

    }

    private void OnPlayerFingerMove(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        if (waitForReset) return;

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

    


    #region ORTHO CAM
    private void PlayerFingerDown(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);
        if (waitForReset) waitForReset = false;
    }

    private void PlayerFingerMove(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        if (waitForReset) return;
        Vector3 diff = CameraInstance.ScreenToWorldPoint(e.ScreenPos) - CameraInstance.transform.position;
        CameraInstance.transform.position = startingPos - diff;
        startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);
        if (e.Phase == UnityEngine.InputSystem.TouchPhase.Stationary)
        {
            
        }

    }

    private void PlayerFingerRelease(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = CameraInstance.ScreenToWorldPoint(e.ScreenPos);
        CurrPos = new Vector3(CameraInstance.transform.position.x, 55, CameraInstance.transform.position.z);
    }
    #endregion

    public void Disable() => DisableMovement();
    public void Enable() => EnableMovement();
}
