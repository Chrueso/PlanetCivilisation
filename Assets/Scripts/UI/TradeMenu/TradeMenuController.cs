using System.Collections.Generic;
using UnityEngine;

public class TradeMenuController
{
    private TradeMenuView view;
    private EntityModel playerModel;
    private DiplomacySystem diplomacySystem;

    // AI Context
    private List<EntityModel> aiModels = new List<EntityModel>();
    private EntityModel currentTargetAI;

    // Internal State
    private int currentFactionIndex = 0;
    private int currentGiveAmount = 0;

    public TradeMenuController(TradeMenuView view, DiplomacySystem diplomacySystem)
    {
        this.view = view;
        this.diplomacySystem = diplomacySystem;

        ConnectView();
        InitializeDropdowns();
    }

    private void ConnectView()
    {
        view.OnCloseClicked = CloseView;
        view.OnChangeFactionClicked = HandleFactionCycle;
        view.OnChangeGiveAmountClicked = HandleAmountChange;
        view.OnConfirmTradeClicked = TryExecuteTrade;
        view.OnResourceDropdownChanged = RefreshView;
    }

    private void InitializeDropdowns()
    {
        List<string> resources = new List<string> {
            ResourceType.Metals.ToString(),
            ResourceType.Rations.ToString()
        };
        view.SetupDropdowns(resources);
    }

    // Now accepts a list of all AI entities so we can cycle through them
    public void OpenView(EntityModel player, List<EntityModel> ais)
    {
        if (player == null || ais == null || ais.Count == 0) return;

        playerModel = player;
        aiModels = ais;
        
        currentFactionIndex = 0;
        currentGiveAmount = 0;
        currentTargetAI = aiModels[currentFactionIndex];

        RefreshView();
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        currentTargetAI = null;
        playerModel = null;
        aiModels.Clear();
    }

    private void HandleFactionCycle(int direction)
    {
        if (aiModels == null || aiModels.Count == 0) return;

        currentFactionIndex += direction;

        // Wrap around array
        if (currentFactionIndex < 0)
            currentFactionIndex = aiModels.Count - 1;
        else if (currentFactionIndex >= aiModels.Count)
            currentFactionIndex = 0;

        currentTargetAI = aiModels[currentFactionIndex];
        currentGiveAmount = 0; // Reset amount when changing faction
        RefreshView();
    }

    private void HandleAmountChange(int delta)
    {
        currentGiveAmount += delta;

        // Clamp below 0 or above what player can afford
        ResourceType selectedGiveResource = view.GetPlayerSelectedResource();
        int maxAffordable = 0;

        if (playerModel != null && playerModel.Resources.TryGetValue(selectedGiveResource, out int currentInv))
        {
            maxAffordable = currentInv;
        }

        currentGiveAmount = Mathf.Clamp(currentGiveAmount, 0, maxAffordable);
        RefreshView();
    }

    private void RefreshView()
    {
        if (currentTargetAI == null)
        {
            view.UpdateView(FactionType.Nothing, "Unknown", 0, 0, false);
            return;
        }

        //Calculate Exchange Rate logic
        float exchangeRate = 1.0f;
        string relationshipString = "NEUTRAL"; // Default until you implement global relationship

        RelationshipLevel rel = RelationshipLevel.NEUTRAL; 

        switch (rel)
        {
            case RelationshipLevel.HOSTILE: exchangeRate = 0.5f; break; // 1:0.5 (bad)
            case RelationshipLevel.NEUTRAL: exchangeRate = 1.0f; break;
            case RelationshipLevel.FRIENDLY: exchangeRate = 2.0f; break; // 1:2 (good)
        }
        

        // Calculate what AI gives
        int aiReceives = (int)Mathf.Floor(currentGiveAmount * exchangeRate);

        // Validate Trade
        ResourceType giveType = view.GetPlayerSelectedResource();
        ResourceType receiveType = view.GetAISelectedResource();

        bool tradeTypesAreDifferent = giveType != receiveType; 
        
        bool aiCanAfford = false;
        if(currentTargetAI.Resources.TryGetValue(receiveType, out int aiHas))
        {
            aiCanAfford = aiHas >= aiReceives;
        }

        bool isValidTrade = tradeTypesAreDifferent && aiCanAfford && currentGiveAmount > 0;

        view.UpdateView(currentTargetAI.FactionType, relationshipString, currentGiveAmount, aiReceives, isValidTrade);
    }

    private void TryExecuteTrade()
    {
        if (playerModel == null || currentTargetAI == null) return;

        ResourceType pGiveType = view.GetPlayerSelectedResource();
        int pGiveAmount = currentGiveAmount;

        ResourceType aiGiveType = view.GetAISelectedResource();

        float exchangeRate = 1.0f;
        
        int aiGiveAmount = (int)Mathf.Floor(pGiveAmount * exchangeRate);

        TradeDeal deal = new TradeDeal(
            trade1_type: aiGiveType,
            trade1_amount: aiGiveAmount,
            trade2_type: pGiveType,
            trade2_amount: pGiveAmount
        );

        // global instead of planet
        if (diplomacySystem.TradeGlobal(currentTargetAI, deal))
        {
            Debug.Log($"Trade successful! Traded {pGiveAmount} {pGiveType} for {aiGiveAmount} {aiGiveType}");
            currentGiveAmount = 0;
            RefreshView();
        }
        else
        {
            Debug.Log($"Trade failed. You or the target might not have enough resources.");
        }
    }
}