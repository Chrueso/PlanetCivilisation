using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public struct TradeDeal
{
    public ResourceType trade1_type; // AI's Offer
    public int trade1_amount;

    public ResourceType trade2_type; // Player's Offer
    public int trade2_amount;

    TradeDeal(ResourceType trade1_type, int trade1_amount, ResourceType trade2_type, int trade2_amount)
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

public class DiplomacySystem : MonoBehaviour 
{
    // trade can only be done by player for now
    // however pretty much this function should get the current person's turn's stuff and check with the PlanetData
    public void Trade(/*Some identifier here,*/PlanetData planetData, TradeDeal trade)
    {
        // Get current person's turn and their identifier to get their data, for now i just hardcode Player from GameManager
        Player player = GameManager.Instance.Player;
        FactionType faction = planetData.FactionType; // who you're trading with

        // i just noticed planet doesnt have inventory but we trading with them lol

        player.GainResource(trade.trade1_type/*, trade.trade1_amount*/); // change after merge
                                                                         // factionGuy or PlanetData.GainResource(trade.trade2_type, trade.trade2_amount);



        // increase affection based on receiving amount
        planetData.RaiseAffection(player.FactionType, Mathf.FloorToInt(trade.trade2_amount*0.1f)); // 90% of trade amount goes to affect for now? best to prolly just clamp it between 1-10
    }

    public Dictionary<FactionType, Dictionary<TradeType, Dictionary<ResourceType, int>>> GetTradeDeals()
    {
        return null;
    }

    public void Gift()
    {
    }

    public void Agreement()
    {

    }
    
    

    
}
