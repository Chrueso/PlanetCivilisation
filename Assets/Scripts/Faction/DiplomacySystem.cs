using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using Mono.Cecil;

public struct TradeDeal
{
    public ResourceType trade1_type; // AI's Offer
    public int trade1_amount;

    public ResourceType trade2_type; // Player's Offer (no consent)
    public int trade2_amount;

    public TradeType tradeType;
    public int affectionCoefficient;
    public TradeDeal(ResourceType trade1_type, int trade1_amount, ResourceType trade2_type, int trade2_amount)
    {
        this.trade1_type = trade1_type;
        this.trade1_amount = trade1_amount;
        this.trade2_type = trade2_type;
        this.trade2_amount = trade2_amount;
        this.tradeType = TradeType.FAIR;
        this.affectionCoefficient = 0;
        if (trade1_amount == trade2_amount)
        {
            tradeType = TradeType.FAIR;
        }
        else if (trade1_amount < trade2_amount)
        {
            tradeType = TradeType.UNFAIR;
            affectionCoefficient = 1;
        } else if (trade1_amount > trade2_amount)
        {
            tradeType = TradeType.UNFAIR;
            affectionCoefficient = -1;
        }
    }
}

public enum TradeType
{
    FAIR,
    UNFAIR
}

public enum PactType
{
    NAP, // Non-Aggression Pact
    FCP, // Faction Conversion Pact

}

public enum RelationshipLevel
{
    HOSTILE,
    INDIFFERENT,
    NEUTRAL,
    FRIENDLY
}

public class DiplomacySystem 
{
    public static Dictionary<ResourceType, int> TradeDeal = new();
    private EventBinding<GameStartEvent> gameStartBinding;
    private EventBinding<TurnChangeEvent> turnChangeEventBinding;

    private FactionType currentFactionTurn = FactionType.Nothing;
    private IEntityController player = default;
    private IEntityController currentEntity = default;
    public DiplomacySystem()
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(OnTurnChanged);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
    }

    private void OnTurnChanged(TurnChangeEvent turnChangeEvent)
    {
        currentFactionTurn = turnChangeEvent.CurrentTurnFaction;
        currentEntity = turnChangeEvent.CurrentEntity;
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        player = gameStartEvent.PlayerController;
    }

    
    // trade can only be done by player for now
    // however pretty much this function should get the current person's turn's stuff and check with the PlanetData
    public bool Trade(PlanetData planetData, TradeDeal trade)
    {
        // do checks here
        // if either guys dont have the amount they are offering, dont let trade happen
        Debug.Log("WAZZAT");
        if (player.GetModel().Resources[trade.trade2_type] < trade.trade2_amount)
        {
            Debug.Log("PUSSY");
            return false;
        }

        // highly unlikely but have to double check just in case
        if (planetData.ResourceInventory[trade.trade1_type] < trade.trade1_amount)
        {
            Debug.Log("SLUT");
            return false;
        }

        // trade happens here
        player.GetModel().TakeResource(trade.trade2_type, trade.trade2_amount);
        player.GetModel().GainResource(trade.trade1_type, trade.trade1_amount);
        planetData.RemoveResource(trade.trade1_type, trade.trade1_amount);
        planetData.GainResource(trade.trade2_type, trade.trade2_amount);

        Debug.Log("WOOHOO");
        // increase affection based on receiving amount
        int affection = Mathf.RoundToInt((trade.trade2_amount*0.5f) + (trade.affectionCoefficient*trade.trade2_amount));
        planetData.RaiseAffection(player.GetModel().FactionType, affection); // 90% of trade amount goes to affect for now? best to prolly just clamp it between 1-10
        return true;
    }
    
    public bool TradeGlobal(EntityModel targetAI, TradeDeal trade)
    {
        //player
        if (player.GetModel().Resources[trade.trade2_type] < trade.trade2_amount)
{
            return false;
}
        // ai
        if (targetAI.Resources[trade.trade1_type] < trade.trade1_amount)
{
            return false;
}
        // trade
        player.GetModel().TakeResource(trade.trade2_type, trade.trade2_amount);
        player.GetModel().GainResource(trade.trade1_type, trade.trade1_amount);

        targetAI.TakeResource(trade.trade1_type, trade.trade1_amount);
        targetAI.GainResource(trade.trade2_type, trade.trade2_amount);

        return true;
    }
    public Dictionary<ResourceType, int> GetTradeDeal(PlanetData planetData, bool threaten = false) // Randomly generated based on planet resource amount
    {
        // this function generates the trade deals that will then be display in the trade popup
        // Get current person's turn and their identifier to get their data, for now i just hardcode Player from GameManager
        Dictionary<ResourceType, int> tradePayload = new();
        List<ResourceType> resources = new List<ResourceType>() { ResourceType.Metals, ResourceType.Rations };
        int rand = UnityEngine.Random.Range(0, resources.Count);
        Debug.Log($"MY NUMBER HUEHUE {rand}");
        ResourceType getResource = resources[rand];
        if (planetData.ResourceInventory.TryGetValue(getResource, out int invAmount))
        {

        }
        
        if (!threaten)
        {
            tradePayload[getResource] = invAmount > 0 ? UnityEngine.Random.Range(1, Mathf.Max(invAmount / 2, 1)) : 0;
        } else
        {
            tradePayload[getResource] = invAmount > 0 ? UnityEngine.Random.Range(Mathf.Max(invAmount / 2, 1), Mathf.Max(invAmount, 1)) : 0;
        }


        return tradePayload;
    }

    public void Gift(PlanetData planetData, ResourceType resource, int amount)
    {
        // do checks here
        // pretty much just gift like depending on multiples of 10
        player.GetModel().TakeResource(resource, amount);
        int affection = Mathf.RoundToInt(amount * 0.5f);
        planetData.GainResource(resource, amount);
        planetData.RaiseAffection(player.GetModel().FactionType, affection); // temporary formula
    }

    public void Agreement(PlanetData planetData, PactType pactType)
    {
        if (pactType == PactType.NAP)
        {
            if (planetData.HasNAPact) return;
            if (planetData.Relations[player.GetModel().FactionType] >= RelationshipLevel.INDIFFERENT)
            {
                planetData.AddPact(PactType.NAP, currentFactionTurn);
            }
        }
        else if (pactType == PactType.FCP)
        {
            if (planetData.Relations[player.GetModel().FactionType] == RelationshipLevel.FRIENDLY)
            {
                planetData.AddPact(pactType, currentFactionTurn);
                player.GetModel().AddOwnedPlanets(planetData);
                planetData.SetFaction(player.GetModel().FactionType);
            }
            
        }
    }
}
