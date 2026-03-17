using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class HandleSafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private DrivenRectTransformTracker tracker;
    private Rect lastSafeArea;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        tracker.Add(this, rectTransform, DrivenTransformProperties.All); //allows this obj to be set as a driver meaning telling that this script will
        //have the allows script to show currect state of rect transform
        Canvas.willRenderCanvases += OnWillRenderCanvases;
    }

    private void OnDisable()
    {
        tracker.Clear();
        Canvas.willRenderCanvases -= OnWillRenderCanvases;
    }

    private void OnWillRenderCanvases()
    {
        //get the device's safe area rectangle in physical pixel
        Rect safeArea = Screen.safeArea;

        if (safeArea == lastSafeArea) return;
        lastSafeArea = safeArea;

        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);

        Vector2 minAchor = safeArea.position / screenResolution;
        Vector2 maxAchor = minAchor + (safeArea.size / screenResolution);

        rectTransform.anchorMin = minAchor;
        rectTransform.anchorMax = maxAchor;
        rectTransform.localScale = Vector3.one;
    }
}