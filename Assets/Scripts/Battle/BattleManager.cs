using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager 
{
    public PlanetData SelectedPlanet;
    public ShipType combatShipType;
    public int AttackShipAmount; 
    public ShipDatabaseSO shipDatabase;

    public BattleManager(ShipDatabaseSO shipDatabase)
    {
        this.shipDatabase = shipDatabase;
    }

    public BattleResult Battle(Dictionary<ShipType, int> attackerShips, PlanetData targetPlanet)
    {
        bool attackerWon = false;
        int maxRoll = 10; // can be adjusted for more or less randomness, the base number for attack power multiplier

        Dictionary<ShipType,int> defenderShips = targetPlanet.StationedShips;

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

    public BattleResult BattleBrianVersion(Dictionary<ShipType, int> attackerShips, PlanetData targetPlanet)
    {
        int attackerShipPower = 0;
        int defenderShipPower = 0;

        Dictionary<ShipType, int> defenderShips = targetPlanet.StationedShips;

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
