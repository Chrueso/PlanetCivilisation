using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StationShips : MonoBehaviour
{
    [SerializeField] private Image[] buttonRows;
    [SerializeField] private Button stationShipsButton;

    private Dictionary<string, int> shipAmount = new Dictionary<string, int>();
    private enum ButtonType
    {
        Min, Decrease, Increase, Max
    }

    // hardcode
    private int playerShip = 2;

    private void OnEnable()
    {
        foreach (Image row in buttonRows)
        {
            shipAmount.Add($"{row.name}", 0);
            foreach (Button buttons in row.GetComponentsInChildren<Button>())
            {
                if (buttons.name.Contains("Increase"))
                {
                    buttons.onClick.AddListener(delegate { IncreaseShip($"{row.name}", ButtonType.Increase, row.GetComponentInChildren<TextMeshProUGUI>()); });
                }
                if (buttons.name.Contains("Max"))
                {
                    buttons.onClick.AddListener(delegate { MaxShip($"{row.name}", ButtonType.Max, row.GetComponentInChildren<TextMeshProUGUI>()); });
                }
                if (buttons.name.Contains("Decrease"))
                {
                    buttons.onClick.AddListener(delegate { DecreaseShip($"{row.name}", ButtonType.Decrease, row.GetComponentInChildren<TextMeshProUGUI>()); });
                } 
                if (buttons.name.Contains("Empty"))
                {
                    buttons.onClick.AddListener(delegate { MinShip($"{row.name}", ButtonType.Min, row.GetComponentInChildren<TextMeshProUGUI>()); });
                }
            }
            row.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        }
        foreach (var kvp in shipAmount)
        {
            print($"Key: {kvp.Key} | Val: {kvp.Value}");
        }
    }

    private void IncreaseShip(string shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        print(shipKey);
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = Math.Clamp(amount + 1, 0, playerShip);
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void MaxShip(string shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = playerShip;
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void DecreaseShip(string shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = Math.Max(amount - 1, 0);
            displayText.text = $"{shipAmount[shipKey]}";
        }
        
    }

    private void MinShip(string shipKey, ButtonType type, TextMeshProUGUI displayText)
    {
        if (shipAmount.TryGetValue(shipKey, out var amount))
        {
            shipAmount[shipKey] = 0;
            displayText.text = $"{shipAmount[shipKey]}";
        }
    }
}
