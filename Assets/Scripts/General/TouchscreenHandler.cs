using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchscreenHandler : MonoBehaviour
{
    private static TouchscreenHandler instance;
    public static TouchscreenHandler main => instance;

    public EventHandler<TouchInfo> FingerDownCallback;
    public EventHandler<TouchInfo> FingerMoveCallback;
    public EventHandler<TouchInfo> FingerUpCallback;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void LateUpdate()
    {
        var activeTouches = Touch.activeTouches;
        foreach (Touch touch in activeTouches) 
        {
            TouchInfo touchInfo = new(currentTouch: touch, touchIndex: touch.displayIndex, lastTouch: touch, screenPos: touch.screenPosition, touch.phase);
            switch (touch.phase)
            {
                case TouchPhase.Began: FingerDownCallback.Invoke(this, touchInfo); break;
                case TouchPhase.Moved: FingerMoveCallback.Invoke(this, touchInfo); break;
                case TouchPhase.Ended: FingerUpCallback.Invoke(this, touchInfo); break;
                case TouchPhase.Stationary: FingerMoveCallback.Invoke(this, touchInfo); break;   
            }
        }
    }


}
