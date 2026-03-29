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

    public TradeDeal(ResourceType trade1_type, int trade1_amount, ResourceType trade2_type, int trade2_amount)
    {
        this.trade1_type = trade1_type;
        this.trade1_amount = trade1_amount;
        this.trade2_type = trade2_type;
        this.trade2_amount = trade2_amount;
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
    public static Dictionary<TradeType, TradeDeal> TradeDeal = new();
    private EventBinding<GameStartEvent> gameStartBinding;
    private EventBinding<TurnChangeEvent> turnChangeEventBinding;

    private FactionType currentFactionTurn = FactionType.Nothing;
    private IEntityController player = default;
    public DiplomacySystem()
    {
        gameStartBinding = new EventBinding<GameStartEvent>(HandleGameStart);
        EventBus<GameStartEvent>.Register(gameStartBinding);
        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(OnTurnChanged);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
    }

    private void OnTurnChanged(TurnChangeEvent turnChangeEvent)
    {
        Debug.Log("WOW U DID SOMETHING");
        currentFactionTurn = turnChangeEvent.CurrentTurnFaction;
    }

    private void HandleGameStart(GameStartEvent gameStartEvent)
    {
        Debug.Log($"{gameStartEvent.PlayerController.GetModel().FactionType}");
        player = gameStartEvent.PlayerController;
    }

    // trade can only be done by player for now
    // however pretty much this function should get the current person's turn's stuff and check with the PlanetData
    public static void Trade(PlanetData planetData, TradeDeal trade)
    {
        // This function is invoked when player clicks on a trade option
        //FactionType faction = planetData.FactionType; // who you're trading with

        // do checks here
        // inventory class within entity maybe?
        //player.RemoveResource(trade.trade2_type, trade.trade2_amount);
        //player.GainResource(trade.trade1_type/*, trade.trade1_amount*/); // change after merge
        // factionGuy or PlanetData.GainResource(trade.trade2_type, trade.trade2_amount);
        planetData.RemoveResource(trade.trade1_type, trade.trade1_amount);
        planetData.GainResource(trade.trade2_type, trade.trade2_amount);


        // increase affection based on receiving amount
        int affection = Mathf.RoundToInt(trade.trade2_amount * 0.5f);
        //planetData.RaiseAffection(player.FactionType, affection); // 90% of trade amount goes to affect for now? best to prolly just clamp it between 1-10
    }

    // "AI" makes the 
    private static Dictionary<TradeType, TradeDeal> GetTradeDeals(/*PlanetData planetData*/) // not really "DECIDED" by ai yet but add parameter for numbers maybe
    {
        // this function generates the trade deals that will then be display in the trade popup
        // Get current person's turn and their identifier to get their data, for now i just hardcode Player from GameManager
        Dictionary<TradeType, TradeDeal> tradePayload = new();
        List<ResourceType> resources = new List<ResourceType>() { ResourceType.Metals, ResourceType.Rations, ResourceType.Credits };
        int rand = UnityEngine.Random.Range(0, resources.Count - 1);
        ResourceType getResource = resources[rand];
        resources.Remove(getResource);
        TradeDeal fairDeal = new TradeDeal(getResource, 10, resources[rand], 10); // amount should be decided by amount in inventory / 10 maybe but for now just like this
        tradePayload[TradeType.FAIR] = fairDeal;
        TradeDeal unfairDeal = new TradeDeal(getResource, 5, resources[rand], 10);
        tradePayload[TradeType.UNFAIR] = unfairDeal;

        return tradePayload;
    }

    public static void GenerateTradeDeal()
    {
        TradeDeal = GetTradeDeals();
    }

    public static void Gift(/*PlanetData planetData,*/ ResourceType resource, int amount)
    {
        // do checks here
        // pretty much just gift like depending on multiples of 10
        
        int affection = Mathf.RoundToInt(amount * 0.5f);
        //planetData.GainResource(resource, amount);
        //planetData.RaiseAffection(GameManager.Instance.Player.FactionType, affection); // temporary formula
    }

    public void Agreement(/*PlanetData planetData,*/ PactType pactType)
    {
        if (pactType == PactType.NAP)
        {
            //if (planetData.HasNAPact) return;

        }
        else if (pactType == PactType.FCP)
        {
            //GameManager.Instance.Player.AddOwnedPlanets(planetData);
            //planetData.SetFaction(GameManager.Instance.Player.FactionType);
        }
        //planetData.AddPact(pactType);
    }
}
