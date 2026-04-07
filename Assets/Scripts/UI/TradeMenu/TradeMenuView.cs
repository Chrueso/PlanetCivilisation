using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeMenuView : ScreenBase
{
    [Header("Main UI")]
    public Button CloseButton;
    public TabGroup tabGroup;

    [Header("Faction Selection")]
    public TMP_Text SelectedFactionText;
    public TMP_Text RelationshipText;
    public Button PrevFactionButton;
    public Button NextFactionButton;

    [Header("TAB 1: TRADING")]
    public Button ConfirmTradeButton;
    public TMP_Text PlayerTradeGiveAmountText;
    public Button PlayerTradeIncreaseBtn;
    public Button PlayerTradeDecreaseBtn;
    public TMP_Dropdown PlayerTradeDropdown;

    public TMP_Text AITradeGiveAmountText;
    public TMP_Dropdown AITradeDropdown;

    [Header("TAB 2: GIFTING")]
    public Button ConfirmGiftButton;
    public TMP_Text PlayerGiftAmountText;
    public Button PlayerGiftIncreaseBtn;
    public Button PlayerGiftDecreaseBtn;
    public TMP_Dropdown PlayerGiftDropdown;

    [Header("TAB 3: PACTS")]
    public Button NAPButton; // Non-Aggression Pact
    public Button FCPButton; // Faction Conversion Pact

    // Callbacks to Controller
    public Action OnCloseClicked;
    public Action<int> OnChangeFactionClicked; // +1 or -1
    
    public Action<int> OnChangeTradeGiveAmountClicked; 
    public Action OnTradeResourceDropdownChanged;
    public Action OnConfirmTradeClicked;

    public Action<int> OnChangeGiftGiveAmountClicked;
    public Action OnGiftResourceDropdownChanged;
    public Action OnConfirmGiftClicked;

    public Action OnNAPClicked;
    public Action OnFCPClicked;

    private void OnEnable()
    {
        CloseButton?.onClick.AddListener(() => OnCloseClicked?.Invoke());
        
        PrevFactionButton?.onClick.AddListener(() => OnChangeFactionClicked?.Invoke(-1));
        NextFactionButton?.onClick.AddListener(() => OnChangeFactionClicked?.Invoke(1));

        // Trade
        ConfirmTradeButton?.onClick.AddListener(() => OnConfirmTradeClicked?.Invoke());
        PlayerTradeIncreaseBtn?.onClick.AddListener(() => OnChangeTradeGiveAmountClicked?.Invoke(1));
        PlayerTradeDecreaseBtn?.onClick.AddListener(() => OnChangeTradeGiveAmountClicked?.Invoke(-1));
        PlayerTradeDropdown?.onValueChanged.AddListener((val) => OnTradeResourceDropdownChanged?.Invoke());
        AITradeDropdown?.onValueChanged.AddListener((val) => OnTradeResourceDropdownChanged?.Invoke());

        // Gift
        ConfirmGiftButton?.onClick.AddListener(() => OnConfirmGiftClicked?.Invoke());
        PlayerGiftIncreaseBtn?.onClick.AddListener(() => OnChangeGiftGiveAmountClicked?.Invoke(1));
        PlayerGiftDecreaseBtn?.onClick.AddListener(() => OnChangeGiftGiveAmountClicked?.Invoke(-1));
        PlayerGiftDropdown?.onValueChanged.AddListener((val) => OnGiftResourceDropdownChanged?.Invoke());

        // Pacts
        NAPButton?.onClick.AddListener(() => OnNAPClicked?.Invoke());
        FCPButton?.onClick.AddListener(() => OnFCPClicked?.Invoke());
    }

    private void OnDisable()
    {
        CloseButton?.onClick.RemoveAllListeners();
        ConfirmTradeButton?.onClick.RemoveAllListeners();
        PrevFactionButton?.onClick.RemoveAllListeners();
        NextFactionButton?.onClick.RemoveAllListeners();
        PlayerTradeIncreaseBtn?.onClick.RemoveAllListeners();
        PlayerTradeDecreaseBtn?.onClick.RemoveAllListeners();
        PlayerTradeDropdown?.onValueChanged.RemoveAllListeners();
        AITradeDropdown?.onValueChanged.RemoveAllListeners();
        
        ConfirmGiftButton?.onClick.RemoveAllListeners();
        PlayerGiftIncreaseBtn?.onClick.RemoveAllListeners();
        PlayerGiftDecreaseBtn?.onClick.RemoveAllListeners();
        PlayerGiftDropdown?.onValueChanged.RemoveAllListeners();
        
        NAPButton?.onClick.RemoveAllListeners();
        FCPButton?.onClick.RemoveAllListeners();
    }

    public void SetupDropdowns(List<string> resourceNames)
    {
        if (PlayerTradeDropdown != null) { PlayerTradeDropdown.ClearOptions(); PlayerTradeDropdown.AddOptions(resourceNames); }
        if (AITradeDropdown != null) { AITradeDropdown.ClearOptions(); AITradeDropdown.AddOptions(resourceNames); }
        if (PlayerGiftDropdown != null) { PlayerGiftDropdown.ClearOptions(); PlayerGiftDropdown.AddOptions(resourceNames); }

        if (resourceNames.Count > 1)
        {
            if (PlayerTradeDropdown != null) PlayerTradeDropdown.value = 0;
            if (AITradeDropdown != null) AITradeDropdown.value = 1;
            if (PlayerGiftDropdown != null) PlayerGiftDropdown.value = 0;
        }
    }

    public ResourceType GetPlayerTradeResource() => PlayerTradeDropdown != null ? (ResourceType)PlayerTradeDropdown.value : ResourceType.Metals;
    public ResourceType GetAITradeResource() => AITradeDropdown != null ? (ResourceType)AITradeDropdown.value : ResourceType.Metals;
    public ResourceType GetPlayerGiftResource() => PlayerGiftDropdown != null ? (ResourceType)PlayerGiftDropdown.value : ResourceType.Metals;

    public void UpdateView(FactionType faction, string relationshipStr, int tradeGiveAmount, int tradeReceiveAmount, int giftAmount, bool tradeIsValid, bool hasInvGift, bool canNAP, bool canFCP)
    {
        if (SelectedFactionText != null) SelectedFactionText.text = faction.ToString();
        if (RelationshipText != null) RelationshipText.text = $"Relationship: {relationshipStr}";

        // Update amounts
        if (PlayerTradeGiveAmountText != null) PlayerTradeGiveAmountText.text = tradeGiveAmount.ToString();
        if (AITradeGiveAmountText != null) AITradeGiveAmountText.text = tradeReceiveAmount.ToString();
        if (PlayerGiftAmountText != null) PlayerGiftAmountText.text = giftAmount.ToString();

        // Control Buttons
        if (ConfirmTradeButton != null) ConfirmTradeButton.interactable = tradeIsValid && tradeGiveAmount > 0;
        if (ConfirmGiftButton != null) ConfirmGiftButton.interactable = hasInvGift && giftAmount > 0;

        // Pact Buttons
        if (NAPButton != null) NAPButton.interactable = canNAP;
        if (FCPButton != null) FCPButton.interactable = canFCP;
    }
}