using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using System;

public class PlanetListView : ScreenBase
{
    public Button CloseButton;

    [Header("Animation")]
    [SerializeField] RectTransform planetListViewPanel;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField, Range(0.1f, 1f)] private float animationOffsetPercentage = 0.2f; // 1f = 100% of screen height
    [SerializeField] private Ease animationEase = Ease.OutCubic;

    private Vector2 originalPosition;
    private float screenHeightOffset;
    private bool hasInitialzedPosition = false;

    public void Init()
    {
        if (planetListViewPanel != null&& !hasInitialzedPosition)
        {
            // just memorizing original position 

            originalPosition = planetListViewPanel.anchoredPosition;
            Canvas canvas = GetComponent<Canvas>();
            screenHeightOffset = canvas.pixelRect.height * animationOffsetPercentage;

            hasInitialzedPosition = true;
            //Debug.Log($"{screenHeight}");

        }
    }

    protected override void OnShow()
    {
        if (planetListViewPanel != null)
        {
            planetListViewPanel.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y - screenHeightOffset);
            planetListViewPanel.DOAnchorPos(originalPosition, animationDuration).SetEase(animationEase);
        }
    }

    protected override void OnHide()
    {
        if (planetListViewPanel != null)
        {
            planetListViewPanel.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y);
            planetListViewPanel.DOAnchorPos(new Vector2(originalPosition.x, originalPosition.y - screenHeightOffset), animationDuration).SetEase(animationEase);

        }
    }
}
