using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;

public class TradeMenuController                            
{
    private TradeMenuView view;
    private EntityModel playerModel;
    private DiplomacySystem diplomacySystem;

    // planet
    private PlanetData planetTradingWith;
    private ResourceType offeredResource;
    private int offeredResourceAmount;

    // AI Context
    private List<EntityModel> aiModels = new List<EntityModel>();
    private EntityModel currentTargetAI;

    // Internal State
    private ResourceType pResourceType;
    private int currentFactionIndex = 0;
    private int currentGiveAmount = 0;

    public TradeMenuController(TradeMenuView view, DiplomacySystem diplomacySystem)
    {
        this.view = view;
        this.diplomacySystem = diplomacySystem;

        ConnectView();
        //InitializeDropdowns();
    }

    private void ConnectView()
    {
        view.OnCloseClicked = CloseView;
        
        // Trade bindings
        view.OnChangeTradeGiveAmountClicked = HandleTradeAmountChange;
        view.OnConfirmTradeClicked = TryExecuteTrade;
        view.OnTradeResourceDropdownChanged = RefreshView;

        // Gift bindings
        view.OnChangeGiftGiveAmountClicked = HandleGiftAmountChange;
        view.OnConfirmGiftClicked = TryExecuteGift;
        view.OnGiftResourceDropdownChanged = RefreshView;

        // Pact bindings
        view.OnNAPClicked = () => TryExecutePact(PactType.NAP);
        view.OnFCPClicked = () => TryExecutePact(PactType.FCP);
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
    public void OpenView(EntityModel player, PlanetData planet)
    {
        if (player == null) return;

        playerModel = player;
        //aiModels = ais;
        
        currentFactionIndex = 0;
        currentGiveAmount = 0;
        planetTradingWith = planet;
        Dictionary<ResourceType, int> tradeDeal = diplomacySystem.GetTradeDeal(planetTradingWith);
        var aiTrade = tradeDeal.First();
        offeredResource = aiTrade.Key;
        offeredResourceAmount = aiTrade.Value;
        pResourceType = offeredResource == ResourceType.Metals ? ResourceType.Rations : ResourceType.Metals;

        RefreshView();
        if (view.tabGroup != null)
        {
            view.tabGroup.JumpToPage(startingTabIndex);
        }

        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        currentPlanet = null;
        planetOwnerAI = null;
        playerModel = null;
        planetTradingWith = null;
        aiModels.Clear();
    }

    private void HandleTradeAmountChange(int delta)
    {
        ResourceType playerGivesType = view.GetPlayerTradeResource();
        int playerMaxAffordable = playerModel != null && playerModel.Resources.TryGetValue(playerGivesType, out int inv) ? inv : 0;

        float exchangeRate = 1.0f;
        if (currentPlanet != null && currentPlanet.Relations != null && currentPlanet.Relations.TryGetValue(playerModel.FactionType, out RelationshipLevel rel))
        {
            if (rel == RelationshipLevel.HOSTILE) exchangeRate = 0.5f;
            else if (rel == RelationshipLevel.FRIENDLY) exchangeRate = 2.0f;
        }

        // max ai can give
        ResourceType aiGivesType = view.GetAITradeResource();
        int aiMaxAffordable = 0;
        if (currentPlanet != null && currentPlanet.ResourceInventory.TryGetValue(aiGivesType, out int planetHas))
        {
            aiMaxAffordable = planetHas;
        }

        // Convert what the AI can afford back into what the PLAYER gives to reach that limit
        // (aiGives = playerGives * exchangeRate) -> (playerGivesLimit = aiGivesMax / exchangeRate)
        int playerGiveLimitForAI = exchangeRate > 0 ? (int)Mathf.Floor(aiMaxAffordable / exchangeRate) : 0;

        // The absolute ceiling is the lowest of either what the player can afford OR what the AI can afford
        int absoluteMax = Mathf.Min(playerMaxAffordable, playerGiveLimitForAI);

        // Apply delta and clamp
        currentTradeGiveAmount += delta;
        currentTradeGiveAmount = Mathf.Clamp(currentTradeGiveAmount, 0, absoluteMax);

        currentTargetAI = null;
        currentGiveAmount = 0; // Reset amount when changing faction
        //RefreshView();
    }

    private void HandleGiftAmountChange(int delta)
    {
        currentGiftGiveAmount += delta;
        ResourceType selectedGiftResource = view.GetPlayerGiftResource();
        int maxAffordable = playerModel != null && playerModel.Resources.TryGetValue(selectedGiftResource, out int inv) ? inv : 0;
        currentGiftGiveAmount = Mathf.Clamp(currentGiftGiveAmount, 0, maxAffordable);
        RefreshView();
    }

    private void RefreshView()
    {
        if (planetTradingWith == null)
        {
            //view.UpdateView(FactionType.Nothing, "Unknown", 0, 0, false);
            return;
        }

        bool isValidTrade = currentGiveAmount > 0 && playerModel.Resources[pResourceType] >= currentGiveAmount;

        //view.UpdateView(currentTargetAI.FactionType, relationshipString, currentGiveAmount, aiReceives, isValidTrade);
        view.UpdateView1(planetTradingWith, pResourceType, currentGiveAmount, offeredResource, offeredResourceAmount, isValidTrade);
    }

    private void TryExecuteTrade()
    {
        //if (playerModel == null || currentTargetAI == null) return;

        ResourceType pGiveType = pResourceType;
        int pGiveAmount = currentGiveAmount;

        float exchangeRate = 1.0f;
        if (currentPlanet.Relations != null && currentPlanet.Relations.TryGetValue(playerModel.FactionType, out RelationshipLevel rel))
        {
            if (rel == RelationshipLevel.HOSTILE) exchangeRate = 0.5f;
            else if (rel == RelationshipLevel.FRIENDLY) exchangeRate = 2.0f;
        }

        int aiGiveAmount = (int)Mathf.Floor(currentTradeGiveAmount * exchangeRate);

        // Execute local trade (Wallet <-> PlanetInventory)
        playerModel.TakeResource(pGiveType, currentTradeGiveAmount);
        playerModel.GainResource(aiGiveType, aiGiveAmount);
        
        currentPlanet.RemoveResource(aiGiveType, aiGiveAmount);
        currentPlanet.GainResource(pGiveType, currentTradeGiveAmount);

        TradeDeal deal = new TradeDeal(
            trade1_type: offeredResource,
            trade1_amount: offeredResourceAmount,
            trade2_type: pGiveType,
            trade2_amount: pGiveAmount
        );

        // global instead of planet
        if (diplomacySystem.Trade(planetTradingWith, deal))
        {
            //Debug.Log($"Trade successful! Traded {pGiveAmount} {pGiveType} for {aiGiveAmount} {aiGiveType}");
            currentGiveAmount = 0;
            RefreshView();
            CloseView();
        }
        else
        {
            Debug.Log($"Trade failed. You or the target might not have enough resources.");
        }
    }
}