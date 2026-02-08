using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchscreenHandler : MonoBehaviour
{
    private static TouchscreenHandler instance;
    public static TouchscreenHandler main => instance;

    public EventHandler TouchscreenHandlerInstanceEnabled;
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

        //TouchscreenHandlerInstanceEnabled.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void OnFingerDown(Finger finger)
    {
        TouchInfo touchInfo = new(currentTouch: finger.currentTouch, touchIndex: finger.index, 
            lastTouch: finger.lastTouch, screenPos: finger.screenPosition, phase: finger.currentTouch.phase);
        FingerDownCallback.Invoke(finger, touchInfo);
        
    }
    private void OnFingerMove(Finger finger)
    {
        TouchInfo touchInfo = new(currentTouch: finger.currentTouch, touchIndex: finger.index,
            lastTouch: finger.lastTouch, screenPos: finger.screenPosition, phase: finger.currentTouch.phase);
        FingerMoveCallback.Invoke(finger, touchInfo);
    }

    private void OnFingerUp(Finger finger)
    {
        TouchInfo touchInfo = new(currentTouch: finger.currentTouch, touchIndex: finger.index,
            lastTouch: finger.lastTouch, screenPos: finger.screenPosition, phase: finger.currentTouch.phase);
        FingerUpCallback.Invoke(finger, touchInfo);
    }

    private void Update()
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
