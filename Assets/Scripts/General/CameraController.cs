using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController main => instance;

    private bool eventsEnabled = true;
    private bool waitForReset = false;
    private Camera cameraInstance;
    private Vector3 startingPos = Vector2.zero;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        } else
        {
            instance = this;
        }
    }

    private void Start()
    {
        cameraInstance = Camera.main;
        TouchscreenHandler.main.FingerDownCallback += PlayerFingerDown;
        TouchscreenHandler.main.FingerMoveCallback += PlayerFingerMove;
        TouchscreenHandler.main.FingerUpCallback += PlayerFingerRelease;
    }

    private void OnDisable()
    {
        TouchscreenHandler.main.FingerDownCallback -= PlayerFingerDown;
        TouchscreenHandler.main.FingerMoveCallback -= PlayerFingerMove;
        TouchscreenHandler.main.FingerUpCallback -= PlayerFingerRelease;
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


    private void PlayerFingerDown(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
        if (waitForReset) waitForReset = false;
    }

    private void PlayerFingerMove(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        if (waitForReset) return;
        Vector3 diff = cameraInstance.ScreenToWorldPoint(e.Pos) - cameraInstance.transform.position;
        cameraInstance.transform.position = startingPos - diff;
        startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
        if (e.Phase == UnityEngine.InputSystem.TouchPhase.Stationary)
        {
            
        }

    }

    private void PlayerFingerRelease(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        if (!eventsEnabled) return;
        startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
    }

    public void Disable() => DisableMovement();
    public void Enable() => EnableMovement();
}
