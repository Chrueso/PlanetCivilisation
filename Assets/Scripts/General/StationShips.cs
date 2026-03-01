using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationShips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DisplayText;
    [SerializeField] private Button IncreaseBtn;
    [SerializeField] private Button MaxBtn;
    [SerializeField] private Button DecreaseBtn;
    [SerializeField] private Button MinBtn;

    private int currShip = 0;

    // hardcode
    private int playerShip = 2;

    private void OnEnable()
    {
        IncreaseBtn.onClick.AddListener(IncreaseShip);
        MaxBtn.onClick.AddListener(MaxShip);
        DecreaseBtn.onClick.AddListener(DecreaseShip);
        MinBtn.onClick.AddListener(MinShip);
        UpdateVisuals();
    }

    private void IncreaseShip()
    {
        currShip = Math.Clamp(currShip + 1, 0, playerShip);
        UpdateVisuals();
    }

    private void MaxShip()
    {
        currShip = playerShip;
        UpdateVisuals();
    }

    private void DecreaseShip()
    {
        currShip = Math.Max(currShip - 1, 0);
        UpdateVisuals();
    }

    private void MinShip()
    {
        currShip = 0;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        DisplayText.text = $"{currShip}";
    }

}
