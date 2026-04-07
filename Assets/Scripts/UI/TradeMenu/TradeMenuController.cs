using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TradeMenuController                            
{
    private TradeMenuView view;
    private EntityModel playerModel;
    private DiplomacySystem diplomacySystem;

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
        InitializeDropdowns();
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
    }

    private void HandleTradeAmountChange(int delta)
    {
        currentTradeGiveAmount += delta;
        ResourceType selectedGiveResource = view.GetPlayerTradeResource();
        int maxAffordable = playerModel != null && playerModel.Resources.TryGetValue(selectedGiveResource, out int inv) ? inv : 0;
        currentTradeGiveAmount = Mathf.Clamp(currentTradeGiveAmount, 0, maxAffordable);
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
        bool isValidTrade = (tradeGiveType != tradeReceiveType) && aiCanAfford;

        // GIFT
        ResourceType giftGiveType = view.GetPlayerGiftResource();
        bool hasInvGift = playerModel.Resources.TryGetValue(giftGiveType, out int pgInv) && pgInv >= currentGiftGiveAmount;

        // PACTS 
        bool canNAP = rel >= RelationshipLevel.INDIFFERENT && !currentPlanet.HasNAPact; 
        bool canFCP = rel == RelationshipLevel.FRIENDLY; 

        // Update UI
        view.UpdateView(currentPlanet.FactionType, relationshipString, currentTradeGiveAmount, aiReceives, currentGiftGiveAmount, isValidTrade, hasInvGift, canNAP, canFCP);
    }

    private void TryExecuteTrade()
    {
        if (playerModel == null || currentPlanet == null || currentTradeGiveAmount <= 0) return;

        ResourceType pGiveType = view.GetPlayerTradeResource();
        ResourceType aiGiveType = view.GetAITradeResource();

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

        // Raise local affection on the specific Planet!
        currentPlanet.RaiseAffection(playerModel.FactionType, Mathf.RoundToInt(currentTradeGiveAmount * 0.5f));

        Debug.Log($"Trade successful with Planet {currentPlanet.PlanetName}! Traded {currentTradeGiveAmount} {pGiveType} for {aiGiveAmount} {aiGiveType}");
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