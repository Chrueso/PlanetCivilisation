using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShipMenuView : ScreenBase
{
    [Header("UI Configuration")]
    [SerializeField] public Button closeButton;
    [SerializeField] private Transform elementsContainer;
    [SerializeField] private StationShipElementView shipElementPrefab;



    private List<StationShipElementView> spawnedElements = new List<StationShipElementView>();

    // Called by the Controller to build up the list visually
    public void Populate(IEnumerable<ShipDataSO> ships, PlanetData planetData, Action<ShipDataSO> onBuildRequested)
    {
        ClearPreviousElements();

        foreach (var shipData in ships)
        {
            StationShipElementView newElement = Instantiate(shipElementPrefab, elementsContainer);
            int currentCount = planetData.GetShipCount(shipData.Type);
            newElement.Setup(shipData, currentCount, onBuildRequested);

            spawnedElements.Add(newElement);
        }
    }

    // Called by the Controller to refresh amounts after a purchase
    public void RefreshCounts(PlanetData planetData)
    {
        foreach (var element in spawnedElements)
        {
            ShipDataSO data = element.GetShipData();
            if (data != null)
            {
                int currentCount = planetData.GetShipCount(data.Type);
                element.UpdateCount(currentCount);
            }
        }
    }

    public void ClearPreviousElements()
    {
        foreach (var element in spawnedElements)
        {
            if (element != null)
            {
                Destroy(element.gameObject);
            }
        }
        spawnedElements.Clear();
    }
}
