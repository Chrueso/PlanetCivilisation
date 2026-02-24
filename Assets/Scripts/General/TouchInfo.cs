using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class TouchInfo
{
    private Touch curr;
    private Touch last;
    private int index;
    private Vector3 screenPosition;
    private TouchPhase phase;

    public Touch Current => curr;
    public Touch Last => last;
    public int Index => index;
    public Vector3 Pos => screenPosition;
    public TouchPhase Phase => phase;

    public TouchInfo(Touch currentTouch, int touchIndex, Touch lastTouch, Vector2 screenPos, TouchPhase phase)
    {
        this.curr = currentTouch;
        this.index = touchIndex;
        this.last = lastTouch;
        this.screenPosition = screenPos;
        this.phase = phase;
    }
}
