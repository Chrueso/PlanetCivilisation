using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// prefab
public class StationShipElement : MonoBehaviour
{
    [SerializeField] private Image shipIcon;
    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private TMP_Text requirementsText;
    [SerializeField] private TMP_Text currentCountText;
    [SerializeField] private Button buildButton;

    private ShipDataSO currentShipData;

    public void Init(ShipDataSO data, int currentCount, Action<ShipType> onStationCallback)
    {
        currentShipData = data;

        shipIcon.sprite = data.Icon;
        shipNameText.text = data.Type.ToString();

        // Format requirements text
        requirementsText.text= "";
        string requirementsStr = "Required Resources";
        foreach (var req in data.RequiredResources)
        {
            requirementsStr += $"{req.Amount} {req.ResourceType}\n";
        }
        requirementsText.text = requirementsStr;

        UpdateCount(currentCount);

        buildButton.onClick.RemoveAllListeners();
        buildButton.onClick.AddListener(() => onStationCallback?.Invoke(currentShipData.Type));
    }

    public void UpdateCount(int count)
    {
        currentCountText.text = $"Stationed: {count}";
    }

    public ShipDataSO GetShipData()
    {
        return currentShipData;
    }
}
