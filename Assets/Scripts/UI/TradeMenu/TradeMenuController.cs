using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
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

    // Target Context
    private PlanetData currentPlanet;
    private EntityModel planetOwnerAI;

    // Internal State
    private int currentTradeGiveAmount = 0;
    private int currentGiftGiveAmount = 0;

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
        currentFactionIndex = 0;
        currentGiveAmount = 0;
        currentTradeGiveAmount = 0;
        currentGiftGiveAmount = 0;
        planetTradingWith = planet;
        currentPlanet = planet;
        Dictionary<ResourceType, int> tradeDeal = diplomacySystem.GetTradeDeal(planetTradingWith);
        var aiTrade = tradeDeal.First();
        offeredResource = aiTrade.Key;
        offeredResourceAmount = aiTrade.Value;
        pResourceType = offeredResource == ResourceType.Metals ? ResourceType.Rations : ResourceType.Metals;
        RefreshView();
        GameScreenManager.Push(view);
    }
    public void OpenView(EntityModel player, PlanetData planet, List<EntityModel> ais, int startingTabIndex = 0)
    {
        if (player == null || planet == null || ais == null) return;

        playerModel = player;
        currentPlanet = planet;
        
        // Find the AI model that owns this planet so we can check relations/give affection
        planetOwnerAI = ais.FirstOrDefault(ai => ai.FactionType == planet.FactionType);
        
        currentTradeGiveAmount = 0;
        currentGiftGiveAmount = 0;

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
        Debug.Log(delta);
        /*
        ResourceType playerGivesType = view.GetPlayerTradeResource();
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
        */
        int playerMaxAffordable = playerModel != null && playerModel.Resources.TryGetValue(pResourceType, out int inv) ? inv : 0;
        // Apply delta and clamp
        currentTradeGiveAmount += delta;
        currentTradeGiveAmount = Mathf.Clamp(currentTradeGiveAmount, 0, playerMaxAffordable);

        //currentTargetAI = null;
        currentGiveAmount = 0; // Reset amount when changing faction
        //RefreshView();
        RefreshView();
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

        bool isValidTrade = currentTradeGiveAmount > 0 && playerModel.Resources[pResourceType] >= currentGiveAmount;

        //view.UpdateView(currentTargetAI.FactionType, relationshipString, currentGiveAmount, aiReceives, isValidTrade);
        
       // eexuan below
        if (currentPlanet == null)
        {
            view.UpdateView(FactionType.Nothing, "Unknown", 0, 0, 0, false, false, false, false);
            return;
        }

        // Get LOCAL relations from the exact planet you clicked on
        RelationshipLevel rel = RelationshipLevel.NEUTRAL; 
        if (currentPlanet.Relations != null && currentPlanet.Relations.TryGetValue(playerModel.FactionType, out RelationshipLevel retrievedRel))
        {
            rel = retrievedRel;
        }

        float exchangeRate = 1.0f;
        string relationshipString = rel.ToString(); 

        switch (rel)
        {
            case RelationshipLevel.HOSTILE: exchangeRate = 0.5f; break; 
            case RelationshipLevel.NEUTRAL: exchangeRate = 1.0f; break;
            case RelationshipLevel.FRIENDLY: exchangeRate = 2.0f; break; 
        }

        // TRADE
        int aiReceives = (int)Mathf.Floor(currentTradeGiveAmount * exchangeRate);
        ResourceType tradeGiveType = view.GetPlayerTradeResource();
        ResourceType tradeReceiveType = view.GetAITradeResource();

        bool aiCanAfford = currentPlanet.ResourceInventory.TryGetValue(tradeReceiveType, out int planetHas) && planetHas >= aiReceives;
        //bool isValidTrade = (tradeGiveType != tradeReceiveType) && aiCanAfford;

        // GIFT
        ResourceType giftGiveType = view.GetPlayerGiftResource();
        bool hasInvGift = playerModel.Resources.TryGetValue(giftGiveType, out int pgInv) && pgInv >= currentGiftGiveAmount;

        // PACTS 
        bool canNAP = rel >= RelationshipLevel.INDIFFERENT && !currentPlanet.HasNAPact; 
        bool canFCP = rel == RelationshipLevel.FRIENDLY; 

        // Update UI
        view.UpdateView(currentPlanet.FactionType, relationshipString, currentTradeGiveAmount, aiReceives, currentGiftGiveAmount, isValidTrade, hasInvGift, canNAP, canFCP);
        view.UpdateView1(planetTradingWith, pResourceType, currentTradeGiveAmount, offeredResource, offeredResourceAmount, isValidTrade);
    }

    private void TryExecuteTrade()
    {
        //if (playerModel == null || currentTargetAI == null) return;

        ResourceType pGiveType = pResourceType;
        int pGiveAmount = currentTradeGiveAmount;    
        

        TradeDeal deal = new TradeDeal(
            trade1_type: offeredResource,
            trade1_amount: offeredResourceAmount,
            trade2_type: pResourceType,
            trade2_amount: currentTradeGiveAmount
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
        // Raise local affection on the specific Planet!
        // currentPlanet.RaiseAffection(playerModel.FactionType, Mathf.RoundToInt(currentTradeGiveAmount * 0.5f));

        //Debug.Log($"Trade successful with Planet {currentPlanet.PlanetName}! Traded {currentTradeGiveAmount} {pGiveType} for {aiGiveAmount} {aiGiveType}");
        currentTradeGiveAmount = 0;
        RefreshView();
    }

    private void TryExecuteGift()
    {
        if (playerModel == null || currentPlanet == null || currentGiftGiveAmount <= 0) return;

        ResourceType giftType = view.GetPlayerGiftResource();

        playerModel.TakeResource(giftType, currentGiftGiveAmount);
        currentPlanet.GainResource(giftType, currentGiftGiveAmount);

        // Raise local affection on the specific Planet!
        currentPlanet.RaiseAffection(playerModel.FactionType, Mathf.RoundToInt(currentGiftGiveAmount * 0.5f));

        Debug.Log($"Gifting successful! Sent {currentGiftGiveAmount} {giftType} to Planet {currentPlanet.PlanetName} ({currentPlanet.FactionType})");
        currentGiftGiveAmount = 0;
        RefreshView();
    }

    private void TryExecutePact(PactType pactType)
    {
        if (currentPlanet == null) return;

        // Apply pact directly to the selected planet
        diplomacySystem.Agreement(currentPlanet, pactType);
        
        Debug.Log($"Invoked {pactType} targeting Planet {currentPlanet.PlanetName}!");
        RefreshView();      
    }
}