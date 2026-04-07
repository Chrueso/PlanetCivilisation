using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        GameScreenManager.Push(view);
    }

    public void CloseView()
    {
        GameScreenManager.Pop();
        currentTargetAI = null;
        playerModel = null;
        planetTradingWith = null;
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

        currentTargetAI = null;
        currentGiveAmount = 0; // Reset amount when changing faction
        //RefreshView();
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
        
        int aiGiveAmount = (int)Mathf.Floor(pGiveAmount * exchangeRate);

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