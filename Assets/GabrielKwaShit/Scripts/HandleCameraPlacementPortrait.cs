using UnityEngine;

[ExecuteAlways]
public class HandleCameraPlacementPortrait : MonoBehaviour
{
    [Header("References")]
    public Transform focusPlane;
    public Camera cam;

    [Header("Design Values")]
    public float focusPlaneDistance = 40;
    public Vector2Int resolution;
    public Rect safeArea;
    public float safeRatio;
    public float invSafeRatio; // just to show
    public float offsetYBottom;
    public float offsetYTop;
    public float yOffset;
    public float zOffset;

    private void Update()
    {
        resolution.x = Screen.width;
        resolution.y = Screen.height;

        safeArea = Screen.safeArea;

        safeRatio = safeArea.height / (float)resolution.y;
        invSafeRatio = 1.0f / safeRatio;

        offsetYBottom = safeArea.y;
        offsetYTop = resolution.y - (safeArea.y + safeArea.height);

        if (cam)
        {
            zOffset = focusPlaneDistance / safeRatio; // or focusPlaneDistance * invSafeRatio
            var opposite = zOffset * Mathf.Atan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            yOffset = (offsetYTop - offsetYBottom) / (float)resolution.y * opposite;

            cam.transform.localPosition = new Vector3(0.0f, yOffset, -zOffset);
        }
    }
}