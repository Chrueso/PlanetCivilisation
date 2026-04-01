using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager 
{
    private ShipDatabaseSO shipDatabase;
    private BattleVisualController battleVisual;

    public BattleManager(ShipDatabaseSO shipDatabase, BattleVisualController battleVisual)
    {
        this.shipDatabase = shipDatabase;
        this.battleVisual = battleVisual;
    }

    public BattleResult Battle(Dictionary<ShipType, int> attackerShips, FactionType attackerFaction, PlanetData targetPlanet)
    {
        bool attackerWon = false;
        int maxRoll = 10; // can be adjusted for more or less randomness, the base number for attack power multiplier
        List<BattleStep> battleSteps = new List<BattleStep>();

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
                    battleSteps.Add(new BattleStep(attacker,defender,true));
                }
                else
                {
                    attackers.Remove(attacker);
                    battleSteps.Add(new BattleStep(attacker, defender, false));
                }

                // End battle if one side is empty
                if (attackers.Count == 0 || defenders.Count == 0) break;
            }
        }

        // Count remaining ships by type for attacker and defender
        int remainingAttackers = attackers.Count;
        int remainingDefenders = defenders.Count;

        attackerWon = remainingAttackers > 0;

        battleVisual.setupBattle(attackerShips, targetPlanet.StationedShips, attackerFaction, targetPlanet, battleSteps);

        Debug.Log($"Battle Result: Attacker Ships Remaining: {remainingAttackers}, Defender Ships Remaining: {remainingDefenders}, Attacker Won: {attackerWon}");
        return new BattleResult(remainingAttackers, remainingDefenders, attackerWon, battleSteps);
    }

    #region don't look
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
        return new BattleResult(0, 0, attackerWon, new List<BattleStep>());
    }
    #endregion
}
