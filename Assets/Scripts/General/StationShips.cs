using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StationShips : MonoBehaviour
{
    [SerializeField] private Image[] buttonRows;
    [SerializeField] private Button stationShipsButton;

    private Dictionary<ShipTypeSO, int> shipAmount = new Dictionary<ShipTypeSO, int>();
    private enum ButtonType
    {
        Min, Decrease, Increase, Max
    }

    // hardcode
    private int playerShip = 2;

    private void OnEnable()
    {
        shipAmount[HardcodeReference.Instance.ScoutShip] = 0;
        shipAmount[HardcodeReference.Instance.AttackShip] = 0;
        shipAmount[HardcodeReference.Instance.WorkerShip] = 0;
        ShipTypeSO currShipRef = null;
        foreach (Image row in buttonRows)
        {
            if (row.name == "WNumRows")
            {
                currShipRef = HardcodeReference.Instance.WorkerShip;
            } else if (row.name =="ANumRows")
            {
                currShipRef = HardcodeReference.Instance.AttackShip;
            } else if (row.name == "SNumRows")
            {
                currShipRef = HardcodeReference.Instance.ScoutShip;
            }
                foreach (Button buttons in row.GetComponentsInChildren<Button>())
                {
                    if (buttons.name.Contains("Increase"))
                    {
                        buttons.onClick.AddListener(delegate { IncreaseShip(currShipRef, ButtonType.Increase, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Max"))
                    {
                        buttons.onClick.AddListener(delegate { MaxShip(currShipRef, ButtonType.Max, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Decrease"))
                    {
                        buttons.onClick.AddListener(delegate { DecreaseShip(currShipRef, ButtonType.Decrease, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Empty"))
                    {
                        buttons.onClick.AddListener(delegate { MinShip(currShipRef, ButtonType.Min, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                }
            row.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        }
        stationShipsButton.onClick.AddListener(StationShipHere);
    }

    private void IncreaseShip(ShipTypeSO shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        print(shipKey);
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = Math.Clamp(amount + 1, 0, playerShip);
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void MaxShip(ShipTypeSO shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = playerShip;
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void DecreaseShip(ShipTypeSO shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = Math.Max(amount - 1, 0);
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void MinShip(ShipTypeSO shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = 0;
            displayText.text = $"{shipAmount[shipKey]}";
        }
    }

    private void StationShipHere()
    {
        EventsHandler.Instance.RunStationingSimulation();
        // add ships
        PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.ScoutShip, shipAmount[HardcodeReference.Instance.ScoutShip]);
        PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.WorkerShip, shipAmount[HardcodeReference.Instance.WorkerShip]);
        PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.AttackShip, shipAmount[HardcodeReference.Instance.AttackShip]);
        shipAmount[HardcodeReference.Instance.ScoutShip] = 0;
        shipAmount[HardcodeReference.Instance.WorkerShip] = 0;
        shipAmount[HardcodeReference.Instance.AttackShip] = 0;
    }
}
