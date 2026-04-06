using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeMenuView : ScreenBase
{
    [Header("Main UI")]
    public Button CloseButton;
    public Button ConfirmTradeButton;

    [Header("Faction Selection")]
    public TMP_Text SelectedFactionText;
    public TMP_Text RelationshipText;
    public Button PrevFactionButton;
    public Button NextFactionButton;

    [Header("Player (You) Give")]
    public TMP_Text PlayerGiveAmountText;
    public Button PlayerGiveIncreaseBtn;
    public Button PlayerGiveDecreaseBtn;
    public TMP_Dropdown PlayerGiveDropdown;

    [Header("AI (Them) Give")]
    public TMP_Text AIGiveAmountText;
    public TMP_Dropdown AIGiveDropdown;

    // Callbacks to Controller
    public Action OnCloseClicked;
    public Action<int> OnChangeFactionClicked; // +1 or -1
    public Action<int> OnChangeGiveAmountClicked; // +1 or -1
    public Action OnResourceDropdownChanged;
    public Action OnConfirmTradeClicked;

    private void OnEnable()
    {
        CloseButton.onClick.AddListener(() => OnCloseClicked?.Invoke());
        ConfirmTradeButton.onClick.AddListener(() => OnConfirmTradeClicked?.Invoke());

        PrevFactionButton.onClick.AddListener(() => OnChangeFactionClicked?.Invoke(-1));
        NextFactionButton.onClick.AddListener(() => OnChangeFactionClicked?.Invoke(1));

        PlayerGiveIncreaseBtn.onClick.AddListener(() => OnChangeGiveAmountClicked?.Invoke(1));
        PlayerGiveDecreaseBtn.onClick.AddListener(() => OnChangeGiveAmountClicked?.Invoke(-1));

        PlayerGiveDropdown.onValueChanged.AddListener((val) => OnResourceDropdownChanged?.Invoke());
        AIGiveDropdown.onValueChanged.AddListener((val) => OnResourceDropdownChanged?.Invoke());
    }

    private void OnDisable()
    {
        CloseButton.onClick.RemoveAllListeners();
        ConfirmTradeButton.onClick.RemoveAllListeners();
        PrevFactionButton.onClick.RemoveAllListeners();
        NextFactionButton.onClick.RemoveAllListeners();
        PlayerGiveIncreaseBtn.onClick.RemoveAllListeners();
        PlayerGiveDecreaseBtn.onClick.RemoveAllListeners();
        PlayerGiveDropdown.onValueChanged.RemoveAllListeners();
        AIGiveDropdown.onValueChanged.RemoveAllListeners();
    }

    public void SetupDropdowns(List<string> resourceNames)
    {
        PlayerGiveDropdown.ClearOptions();
        AIGiveDropdown.ClearOptions();

        PlayerGiveDropdown.AddOptions(resourceNames);
        AIGiveDropdown.AddOptions(resourceNames);

        // Default selection avoidance
        if (resourceNames.Count > 1)
        {
            PlayerGiveDropdown.value = 0;
            AIGiveDropdown.value = 1;
        }
    }

    public ResourceType GetPlayerSelectedResource() => (ResourceType)PlayerGiveDropdown.value;
    public ResourceType GetAISelectedResource() => (ResourceType)AIGiveDropdown.value;

    public void UpdateView(FactionType faction, string relationshipStr, int giveAmount, int receiveAmount, bool tradeIsValid)
    {
        SelectedFactionText.text = faction.ToString();
        RelationshipText.text = $"Relationship: {relationshipStr}";

        PlayerGiveAmountText.text = giveAmount.ToString();
        AIGiveAmountText.text = receiveAmount.ToString();

        // Prevent trading identical resources or if trade amount is 0
        ConfirmTradeButton.interactable = tradeIsValid && giveAmount > 0;
    }
}