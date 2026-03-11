using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public PlanetData SelectedPlanet;
    public ShipType combatShipType;
    public int AttackShipAmount; 
    public ShipDatabaseSO shipDatabase;

    //debug/testing functions

    [ContextMenu("Simulate Battle")]
    public void SimulateBattle()
    {
        if (SelectedPlanet != null)
        {
            Dictionary<ShipType, int> attackingFleet = new Dictionary<ShipType, int>();
            attackingFleet.Add(combatShipType, AttackShipAmount);

            Debug.Log($"Starting war against {SelectedPlanet.PlanetName}\n Attacker Ships : {AttackShipAmount} \n ");
            string fleetInfo = string.Join(", ", SelectedPlanet.StationedShips.Select(kv => $"{kv.Key}: {kv.Value}"));
            Debug.Log($"Defending Fleet: {fleetInfo}");

            BattleResult result = Battle(attackingFleet, SelectedPlanet.StationedShips);
        }
    }

    [ContextMenu("Simulate Brian Version")]
    public void SimulateBattleBrian()
    {
        if (SelectedPlanet != null)
        {
            Dictionary<ShipType, int> attackingFleet = new Dictionary<ShipType, int>();
            attackingFleet.Add(combatShipType, AttackShipAmount);

            Debug.Log($"Starting war against {SelectedPlanet.PlanetName}\n Attacker Ships : {AttackShipAmount} \n ");
            string fleetInfo = string.Join(", ", SelectedPlanet.StationedShips.Select(kv => $"{kv.Key}: {kv.Value}"));
            Debug.Log($"Defending Fleet: {fleetInfo}");

            BattleResult result = BattleManager.Instance.BattleBrianVersion(attackingFleet, SelectedPlanet.StationedShips);
        }
    }

    //[ContextMenu("Simulate Brian Version")]
    //public void SimulateBrianVersion()
    //{
    //    Dictionary<ShipType, int> attackerDict = attackerFleet.ToDictionary(e => e.GetShipType(), e => e.count);
    //    Dictionary<ShipType, int> defenderDict = defenderFleet.ToDictionary(e => e.GetShipType(), e => e.count);
    //    BattleResult result = BattleManager.Instance.BattleBrianVersion(attackerDict, defenderDict);
    //    //Debug.Log($"Attacker Won: {result.AttackerWon}");
    //}

    protected override void Awake()
    {
        base.Awake();
    }

    public BattleResult Battle(Dictionary<ShipType, int> attackerShips, Dictionary<ShipType, int> defenderShips)
    {
        bool attackerWon = false;
        int maxRoll = 10;

        List<ShipType> attackers = new List<ShipType>();

        foreach (var kv in attackerShips)
        {
            if (kv.Key == ShipType.Attacker)
            {
                for (int i = 0; i < kv.Value; i++)
                    attackers.Add(kv.Key);
            }
        }

        List<ShipType> defenders = new List<ShipType>();
        foreach (var kv in defenderShips)
        {
            for (int i = 0; i < kv.Value; i++)
                defenders.Add(kv.Key);
        }

        // Battle loop
        while (attackers.Count > 0 && defenders.Count > 0)
        {

            List<ShipType> currentAttackers = new List<ShipType>(attackers);

            foreach (var attacker in currentAttackers)
            {
                if (defenders.Count == 0) break;

                // Pick a random defender
                int defenderIndex = Random.Range(0, defenders.Count);
                ShipType defender = defenders[defenderIndex];

                // Roll
                int attackerPower = shipDatabase.GetShip(attacker).AttackPower;
                int defenderPower = shipDatabase.GetShip(defender).AttackPower;

                int attackerRoll = Random.Range(1, maxRoll * attackerPower + 1);
                int defenderRoll = Random.Range(1, maxRoll * defenderPower + 1);

                if (attackerRoll > defenderRoll)
                {
                    defenders.RemoveAt(defenderIndex);
                }
                else
                {
                    attackers.Remove(attacker);
                }

                // End battle if one side is empty
                if (attackers.Count == 0 || defenders.Count == 0) break;
            }
        }

        // Count remaining ships by type for attacker and defender
        int remainingAttackers = attackers.Count;
        int remainingDefenders = defenders.Count;

        attackerWon = remainingAttackers > 0;

        Debug.Log($"Battle Result: Attacker Ships Remaining: {remainingAttackers}, Defender Ships Remaining: {remainingDefenders}, Attacker Won: {attackerWon}");
        return new BattleResult(remainingAttackers, remainingDefenders, attackerWon);
    }

    public BattleResult BattleBrianVersion(Dictionary<ShipType, int> attackerShips, Dictionary<ShipType, int> defenderShips)
    {
        int attackerShipPower = 0;
        int defenderShipPower = 0;

        foreach (var ship in attackerShips)
        {
            if (ship.Key == ShipType.Attacker)
            {
                int attackPower = shipDatabase.GetShip(ship.Key).AttackPower;
                attackerShipPower += ship.Value * attackPower;
            }
        }

        foreach (var ship in defenderShips)
        {
            int attackPower = shipDatabase.GetShip(ship.Key).AttackPower;
            defenderShipPower += ship.Value * attackPower;
        }

        float chanceForSuccess = (float)attackerShipPower / (attackerShipPower + defenderShipPower);

        bool attackerWon = Random.value < chanceForSuccess;

        Debug.Log($"Attacker Won: {attackerWon} at {chanceForSuccess * 100}%");
        return new BattleResult(0, 0, attackerWon);
    }
}
