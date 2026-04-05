using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class TouchInfo
{
    private Touch curr;
    private Touch last;
    private int index;
    private int fingerIndex;
    private Vector3 screenPosition;
    private TouchPhase phase;
    public Vector3 lastScreenPos;
    

    public Touch Current => curr;
    public Touch Last => last;
    public int Index => index;
    public int FingerId => fingerIndex;
    public Vector3 ScreenPos => screenPosition;
    public TouchPhase Phase => phase;

    // debug
    public TouchInfo()
    {
        this.screenPosition = Vector3.zero;
        this.lastScreenPos = Vector3.zero;
    }
    public TouchInfo(Touch currentTouch, int touchIndex, int fingerIndex, Touch lastTouch, Vector2 screenPos, TouchPhase phase)
    {
        this.curr = currentTouch;
        this.index = touchIndex;
        this.fingerIndex = fingerIndex;
        this.last = lastTouch;
        this.screenPosition = screenPos;
        this.phase = phase;
    }

    public override string ToString()
    {
        return $"Touch {this.curr}, Index {this.index}, finger id {this.fingerIndex}, last {this.last}, pos {this.screenPosition}, phase {this.phase}";
    }
}
