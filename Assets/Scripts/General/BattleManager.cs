using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BattleResult
{
    public int AttackerShipsRemaining { get; private set; }
    public int DefenderShipsRemaining { get; private set; }
    public bool AttackerWon { get; private set; }

    public BattleResult(int attackerShipsRemaining, int defenderShipsRemaining, bool attackerWon)
    {
        AttackerShipsRemaining = attackerShipsRemaining;
        DefenderShipsRemaining = defenderShipsRemaining;
        AttackerWon = attackerWon;
    }
}

public class BattleManager : Singleton<BattleManager>
{
    public PlanetData SelectedPlanet;
    public ShipTypeSO combatShipType;
    public int AttackShipAmount; 

    //debug/testing functions

    [ContextMenu("Simulate Battle")]
    public void SimulateBattle()
    {
        if (SelectedPlanet != null)
        {
            Dictionary<ShipTypeSO, int> attackingFleet = new Dictionary<ShipTypeSO, int>();
            attackingFleet.Add(combatShipType, AttackShipAmount);

            Debug.Log($"Starting war against {SelectedPlanet.PlanetName}\n Attacker Ships : {AttackShipAmount} \n ");
            string fleetInfo = string.Join(", ", SelectedPlanet.StationedShips.Select(kv => $"{kv.Key.name}: {kv.Value}"));
            Debug.Log($"Defending Fleet: {fleetInfo}");

            BattleResult result = BattleManager.Instance.Battle(attackingFleet, SelectedPlanet.StationedShips);
        }
    }

    [ContextMenu("Simulate Brian Version")]
    public void SimulateBattleBrian()
    {
        if (SelectedPlanet != null)
        {
            Dictionary<ShipTypeSO, int> attackingFleet = new Dictionary<ShipTypeSO, int>();
            attackingFleet.Add(combatShipType, AttackShipAmount);

            Debug.Log($"Starting war against {SelectedPlanet.PlanetName}\n Attacker Ships : {AttackShipAmount} \n ");
            string fleetInfo = string.Join(", ", SelectedPlanet.StationedShips.Select(kv => $"{kv.Key.name}: {kv.Value}"));
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

    public BattleResult Battle(Dictionary<ShipTypeSO, int> attackerShips, Dictionary<ShipTypeSO, int> defenderShips)
    {
        bool attackerWon = false;
        int maxRoll = 10;

        List<ShipTypeSO> attackers = new List<ShipTypeSO>();
        foreach (var kv in attackerShips)
        {
            if (kv.Key.ActionType == ShipActionType.Combat)
            {
                for (int i = 0; i < kv.Value; i++)
                    attackers.Add(kv.Key);
            }
        }

        List<ShipTypeSO> defenders = new List<ShipTypeSO>();
        foreach (var kv in defenderShips)
        {
            for (int i = 0; i < kv.Value; i++)
                defenders.Add(kv.Key);
        }

        // Battle loop
        while (attackers.Count > 0 && defenders.Count > 0)
        {

            List<ShipTypeSO> currentAttackers = new List<ShipTypeSO>(attackers);

            foreach (var attacker in currentAttackers)
            {
                if (defenders.Count == 0) break;

                // Pick a random defender
                int defenderIndex = Random.Range(0, defenders.Count);
                ShipTypeSO defender = defenders[defenderIndex];

                // Roll
                int attackerRoll = Random.Range(1, maxRoll * attacker.AttackPower + 1);
                int defenderRoll = Random.Range(1, maxRoll * defender.AttackPower + 1);

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

    public BattleResult BattleBrianVersion(Dictionary<ShipTypeSO, int> attackerShips, Dictionary<ShipTypeSO, int> defenderShips)
    {
        int attackerShipPower = 0;
        int defenderShipPower = 0;

        foreach (var ship in attackerShips)
        {
            if (ship.Key.ActionType == ShipActionType.Combat)
            {
                attackerShipPower += ship.Value * ship.Key.AttackPower;
            }
        }

        foreach (var ship in defenderShips)
        {
            defenderShipPower += ship.Value * ship.Key.AttackPower;
        }

        float chanceForSuccess = (float)attackerShipPower / (attackerShipPower + defenderShipPower);

        bool attackerWon = Random.value < chanceForSuccess;

        Debug.Log($"Attacker Won: {attackerWon} at {chanceForSuccess * 100}%");
        return new BattleResult(0, 0, attackerWon);
    }
}
