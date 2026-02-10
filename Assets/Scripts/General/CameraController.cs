using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Camera cameraInstance;
    private Vector3 startingPos = Vector2.zero;
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



    private void PlayerFingerDown(object sender, TouchInfo e)
    {
        startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
    }

    private void PlayerFingerMove(object sender, TouchInfo e)
    {

        Vector3 diff = cameraInstance.ScreenToWorldPoint(e.Pos) - cameraInstance.transform.position;
        cameraInstance.transform.position = startingPos - diff;
        if (e.Phase == UnityEngine.InputSystem.TouchPhase.Stationary)
        {
            startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
        }

    }

    private void PlayerFingerRelease(object sender, TouchInfo e)
    {
        startingPos = cameraInstance.ScreenToWorldPoint(e.Pos);
    }
}
