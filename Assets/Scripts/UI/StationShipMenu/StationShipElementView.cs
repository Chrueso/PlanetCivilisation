using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// prefab
public class StationShipElementView : MonoBehaviour
{
    [SerializeField] private Image shipIcon;
    [SerializeField] private TMP_Text shipNameText;
    [SerializeField] private TMP_Text requirementsText;
    [SerializeField] private TMP_Text currentCountText;
    [SerializeField] private Button buildButton;

    private ShipDataSO currentShipData;

    public void Setup(ShipDataSO data, int currentCount, Action<ShipDataSO> onBuildCallback)
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
        buildButton.onClick.AddListener(() => onBuildCallback?.Invoke(currentShipData));
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
