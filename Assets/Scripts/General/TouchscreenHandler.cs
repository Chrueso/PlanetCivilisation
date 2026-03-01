using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchscreenHandler : Singleton<TouchscreenHandler>
{

    public EventHandler<TouchInfo> FingerDownCallback;
    public EventHandler<TouchInfo> FingerMoveCallback;
    public EventHandler<TouchInfo> FingerUpCallback;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        var activeTouches = Touch.activeTouches;
        foreach (Touch touch in activeTouches) 
        {
            TouchInfo touchInfo = new(
                currentTouch: touch, 
                touchIndex: touch.displayIndex, 
                lastTouch: touch, 
                screenPos: touch.screenPosition, 
                touch.phase);

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
