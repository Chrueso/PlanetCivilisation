using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlanetListView : ScreenBase
{
    public Button CloseButton;

    [Header("List Setup")]
    [SerializeField] private Transform layoutGroup;
    [SerializeField] private PlanetListElement planetListElement;

    [Header("Animation")]
    [SerializeField] RectTransform planetListViewPanel;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField, Range(0.1f, 1f)] private float animationOffsetPercentage = 0.2f; // 1f = 100% of screen height
    [SerializeField] private Ease animationEase = Ease.OutCubic;

    private Vector2 originalPosition;
    private float screenHeightOffset;
    private bool hasInitialzedPosition = false;

    // Cache spawned rows
    private List<GameObject> activeRows = new List<GameObject>();
    public void Init()
    {
        if (planetListViewPanel != null && !hasInitialzedPosition)
        {
            // just memorizing original position 

            originalPosition = planetListViewPanel.anchoredPosition;
            Canvas canvas = GetComponent<Canvas>();
            screenHeightOffset = canvas.pixelRect.height * animationOffsetPercentage;

            hasInitialzedPosition = true;
            //Debug.Log($"{screenHeight}");

        }
    }

    public void InitalizeList(HashSet<PlanetData> ownedPlanets, Action<PlanetData> onPlanetClicked, Action<PlanetData> onTeleportToPlanetClicked)
    {
        // Destroy old UI rows
        foreach (var row in activeRows)
        {
            Destroy(row);
        }
        activeRows.Clear();

        // Spawn a new row for every planet the player owns
        foreach (PlanetData planet in ownedPlanets)
        {
            PlanetListElement row = Instantiate(planetListElement, layoutGroup);
            row.Init(planet, onPlanetClicked, onTeleportToPlanetClicked); // Fill it with data
            activeRows.Add(row.gameObject);
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
