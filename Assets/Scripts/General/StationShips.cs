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
    private Dictionary<string, ShipTypeSO> shipLookup = new Dictionary<string, ShipTypeSO>();
    private enum ButtonType
    {
        Min, Decrease, Increase, Max
    }

    // hardcode
    private int playerShip = 2;

    private void Start()
    {
        shipAmount[HardcodeReference.Instance.ScoutShip] = 0;
        shipAmount[HardcodeReference.Instance.AttackShip] = 0;
        shipAmount[HardcodeReference.Instance.WorkerShip] = 0;
        shipLookup["WNumRows"] = HardcodeReference.Instance.WorkerShip;
        shipLookup["ANumRows"] = HardcodeReference.Instance.AttackShip;
        shipLookup["SNumRows"] = HardcodeReference.Instance.ScoutShip;
        foreach (Image row in buttonRows)
        {
            
                foreach (Button buttons in row.GetComponentsInChildren<Button>())
                {
                    if (buttons.name.Contains("Increase"))
                    {
                        buttons.onClick.AddListener(delegate { IncreaseShip(shipLookup[row.name], ButtonType.Increase, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Max"))
                    {
                        buttons.onClick.AddListener(delegate { MaxShip(shipLookup[row.name], ButtonType.Max, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Decrease"))
                    {
                        buttons.onClick.AddListener(delegate { DecreaseShip(shipLookup[row.name], ButtonType.Decrease, row.GetComponentInChildren<TextMeshProUGUI>()); });
                    }
                    if (buttons.name.Contains("Empty"))
                    {
                        buttons.onClick.AddListener(delegate { MinShip(shipLookup[row.name], ButtonType.Min, row.GetComponentInChildren<TextMeshProUGUI>()); });
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
            
            shipAmount[shipKey] = Math.Clamp(amount + 1, 0, GetPlayerShipAmount(shipKey));
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void MaxShip(ShipTypeSO shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = GetPlayerShipAmount(shipKey);
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

    private int GetPlayerShipAmount(ShipTypeSO key)
    {
        if (GameManager.Instance.Player.Ships.TryGetValue(key, out var shipAmount))
        {
            return shipAmount;
        }
        return 0;
    }

    private void StationShipHere()
    {
        EventsHandler.Instance.RunStationingSimulation();
        // add ships
        //PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.ScoutShip, shipAmount[HardcodeReference.Instance.ScoutShip]);
        //PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.WorkerShip, shipAmount[HardcodeReference.Instance.WorkerShip]);
        //PlayerInteractionController.Instance.CurrentPlanet.AddShips(HardcodeReference.Instance.AttackShip, shipAmount[HardcodeReference.Instance.AttackShip]);
        PlayerInteractionController.Instance.CurrentPlanet.StationShips(shipAmount);
        shipAmount[HardcodeReference.Instance.ScoutShip] = 0;
        shipAmount[HardcodeReference.Instance.WorkerShip] = 0;
        shipAmount[HardcodeReference.Instance.AttackShip] = 0;
    }
}
