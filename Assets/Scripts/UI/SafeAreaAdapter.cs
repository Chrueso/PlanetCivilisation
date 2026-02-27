using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdapter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    private Vector2Int lastScreenSize;

    // Minimum inset ratios for devices that don't report accurate safe areas
    private const float MIN_BOTTOM_RATIO = 0.035f;
    private const float MIN_TOP_RATIO = 0f; // top is usually reported correctly

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Refresh();
    }

    private void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        Refresh();
    }

    private void Update()
    {
        if (Screen.safeArea != lastSafeArea ||
            Screen.width != lastScreenSize.x ||
            Screen.height != lastScreenSize.y)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        if (Screen.width <= 0 || Screen.height <= 0) return;

        float bottomRatio = Mathf.Max(safeArea.yMin / Screen.height, MIN_BOTTOM_RATIO);
        float topRatio = Mathf.Min(safeArea.yMax / Screen.height, 1f - MIN_TOP_RATIO);

        rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, bottomRatio);
        rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, topRatio);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
