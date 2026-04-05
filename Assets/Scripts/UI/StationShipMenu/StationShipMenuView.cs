using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationShipMenuView : ScreenBase
{
    [Header("UI Configuration")]
    [SerializeField] public Button CloseButton;
    [SerializeField] private Transform elementsContainer;
    [SerializeField] private StationShipElement shipElementPrefab;

    private List<StationShipElement> stationShipElements = new List<StationShipElement>();

    public void Init(IEnumerable<ShipDataSO> ships, PlanetData planetData, Action<ShipType> onStationRequested)
    {
        ClearPreviousElements();

        foreach (var shipData in ships)
        {
            if (shipData.Type == ShipType.Scout) continue; 
            StationShipElement element = Instantiate(shipElementPrefab, elementsContainer);
            int currentCount = planetData.GetShipCount(shipData.Type);
            element.Init(shipData, currentCount, onStationRequested);

            stationShipElements.Add(element);
        }
    }

    public void UpdateView(PlanetData planetData)
    {
        foreach (var element in stationShipElements)
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
        foreach (var element in stationShipElements)
        {
            if (element != null)
            {
                Destroy(element.gameObject);
            }
        }
        stationShipElements.Clear();
    }
}
