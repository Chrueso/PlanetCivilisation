//using System;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using UnityEditor;

//public struct TradeDeal
//{
//    public ResourceType trade1_type; // AI's Offer
//    public int trade1_amount;

//    public ResourceType trade2_type; // Player's Offer (no consent)
//    public int trade2_amount;

//    public TradeDeal(ResourceType trade1_type, int trade1_amount, ResourceType trade2_type, int trade2_amount)
//    {
//        this.trade1_type = trade1_type;
//        this.trade1_amount = trade1_amount;
//        this.trade2_type = trade2_type;
//        this.trade2_amount = trade2_amount;
//    }
//}

//public enum TradeType
//{
//    FAIR,
//    UNFAIR
//}

//public enum PactType
//{
//    NAP, // Non-Aggression Pact
//    FCP, // Faction Conversion Pact

//}

//public enum RelationshipLevel
//{
//    HOSTILE,
//    INDIFFERENT,
//    NEUTRAL,
//    FRIENDLY
//}

//public class DiplomacySystem : MonoBehaviour 
//{

//    private void Start()
//    {
//        Dictionary<TradeType, TradeDeal> tradeSimul = GetTradeDeals();

//        foreach(var trade in tradeSimul)
//        {
//            print($"Trade type: {trade.Key} | Demihuman Trade: {trade.Value.trade1_type} Amount: {trade.Value.trade1_amount} | Human Trade {trade.Value.trade2_type} Amount: {trade.Value.trade2_amount}");
//            Trade(trade.Value);
//        }

//        Gift(ResourceType.Metals, 10);
//        Gift(ResourceType.Rations, 100);
//        Gift(ResourceType.Credits, 1000);
//    }
//    // trade can only be done by player for now
//    // however pretty much this function should get the current person's turn's stuff and check with the PlanetData
//    public static void Trade(/*Some identifier here,PlanetData planetData,*/ TradeDeal trade)
//    {
//        // This function is invoked when player clicks on a trade option
//        // Get current person's turn and their identifier to get their data, for now i just hardcode Player from GameManager
//        Player player = GameManager.Instance.Player;
//        //FactionType faction = planetData.FactionType; // who you're trading with

//        // i just noticed planet doesnt have inventory but we trading with them lol

//        //player.GainResource(trade.trade1_type/*, trade.trade1_amount*/); // change after merge
//                                                                         // factionGuy or PlanetData.GainResource(trade.trade2_type, trade.trade2_amount);
        


//        // increase affection based on receiving amount
//        int affection = Mathf.RoundToInt(trade.trade2_amount * 0.5f);

//        print($"Demihuman Offer: {trade.trade1_type} with amount {trade.trade1_amount} | Human Offer: {trade.trade2_type} with amount {trade.trade2_amount} accepted | The planet gains {affection} affection");
//        //planetData.RaiseAffection(player.FactionType, affection); // 90% of trade amount goes to affect for now? best to prolly just clamp it between 1-10
//    }

//    // "AI" makes the 
//    public static Dictionary<TradeType, TradeDeal> GetTradeDeals(/*PlanetData planetData*/) // not really "DECIDED" by ai yet but add parameter for numbers maybe
//    {
//        // this function generates the trade deals that will then be display in the trade popup
//        // Get current person's turn and their identifier to get their data, for now i just hardcode Player from GameManager
//        Dictionary<TradeType, TradeDeal> tradePayload = new();
//        List<ResourceType> resources = new List<ResourceType>() { ResourceType.Metals, ResourceType.Rations, ResourceType.Credits };
//        int rand = UnityEngine.Random.Range(0, resources.Count-1);
//        ResourceType getResource = resources[rand];
//        resources.Remove(getResource);
//        TradeDeal fairDeal = new TradeDeal(getResource, 10, resources[rand], 10); // amount should be decided by amount in inventory / 10 maybe but for now just like this
//        tradePayload[TradeType.FAIR] = fairDeal;
//        TradeDeal unfairDeal = new TradeDeal(getResource, 5, resources[rand], 10);
//        tradePayload[TradeType.UNFAIR] = unfairDeal;

//        return tradePayload;
//    }

//    public static void Gift(/*PlanetData planetData,*/ ResourceType resource, int amount)
//    {
//        // pretty much just gift like depending on multiples of 10
//        // atm planet data no resource inventory or actually idek if it adds to planet or the person
//        int affection = Mathf.RoundToInt(amount * 0.5f);
//        print($"Human gifted {resource} with amount {amount} to Demihuman faction");
//        //planetData.RaiseAffection(GameManager.Instance.Player.FactionType, affection); // temporary formula
//    }

//    public void Agreement(/*PlanetData planetData,*/ PactType pactType)
//    {
//        if (pactType == PactType.NAP)
//        {
//            //if (planetData.HasNAPact) return;

//        } else if (pactType == PactType.FCP)
//        {
//            //GameManager.Instance.Player.AddOwnedPlanets(planetData);
//            //planetData.SetFaction(GameManager.Instance.Player.FactionType);
//        }
//        //planetData.AddPact(pactType);
//    }
//}
