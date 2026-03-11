using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Text Color")]
    public Color normalColor = new Color(0f, 0.898f, 1f);      // cyan
    public Color hoverColor = Color.white;
    public Color pressedColor = new Color(1f, 0.843f, 0f);     // gold

    private TextMeshProUGUI label;

    private void Awake()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (label) label.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (label) label.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (label) label.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (label) label.color = hoverColor;
    }
}
