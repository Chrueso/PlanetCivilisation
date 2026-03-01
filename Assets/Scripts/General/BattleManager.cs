//using System.Collections.Generic;
//using System.Linq;
//using Unity.VisualScripting;
//using UnityEngine;

//public class BattleResult
//{
//    public int AttackerShipsRemaining { get; private set; }
//    public int DefenderShipsRemaining { get; private set; }
//    public bool AttackerWon { get; private set; }
    
//    public BattleResult(int attackerShipsRemaining, int defenderShipsRemaining, bool attackerWon)
//    {
//        AttackerShipsRemaining = attackerShipsRemaining;
//        DefenderShipsRemaining = defenderShipsRemaining;
//        AttackerWon = attackerWon;
//    }
//}

//public class BattleManager : Singleton<BattleManager>
//{
//    public enum ShipTypeEnum
//    {
//        Scout,
//        Assault,
//        Worker
//    }

//    [System.Serializable]
//    public class ShipEntry
//    {
//        public ShipTypeEnum shipTypeEnum; // Choose in Inspector
//        public int count;

//        public ShipType GetShipType()
//        {
//            return shipTypeEnum switch
//            {
//                ShipTypeEnum.Scout => ShipTypes.Scout,
//                ShipTypeEnum.Assault => ShipTypes.Assault,
//                ShipTypeEnum.Worker => ShipTypes.Worker,
//                _ => null
//            };
//        }
//    }

//    [SerializeField] private List<ShipEntry> attackerFleet = new();
//    [SerializeField] private List<ShipEntry> defenderFleet = new();

//    [ContextMenu("Simulate Battle")]
//    public void SimulateBattle()
//    {
//        Dictionary<ShipType, int> attackerDict = attackerFleet.ToDictionary(e => e.GetShipType(), e => e.count);
//        Dictionary<ShipType, int> defenderDict = defenderFleet.ToDictionary(e => e.GetShipType(), e => e.count);
//        BattleResult result = BattleManager.Instance.Battle(attackerDict, defenderDict);
//        //Debug.Log($"Attacker Ships Remaining: {result.AttackerShipsRemaining}, Defender Ships Remaining: {result.DefenderShipsRemaining}, Attacker Won: {result.AttackerWon}");
//    }

//    [ContextMenu("Simulate Brian Version")]
//    public void SimulateBrianVersion()
//    {
//        Dictionary<ShipType, int> attackerDict = attackerFleet.ToDictionary(e => e.GetShipType(), e => e.count);
//        Dictionary<ShipType, int> defenderDict = defenderFleet.ToDictionary(e => e.GetShipType(), e => e.count);
//        BattleResult result = BattleManager.Instance.BattleBrianVersion(attackerDict, defenderDict);
//        //Debug.Log($"Attacker Won: {result.AttackerWon}");
//    }

//    protected override void Awake()
//    {
//        base.Awake();
//    }

//    public BattleResult Battle(Dictionary<ShipType, int> attackerShips, Dictionary<ShipType, int> defenderShips)
//    {
//        bool attackerWon = false;
//        int maxRoll = 10; 

//        List<ShipType> attackers = new List<ShipType>();
//        foreach (var kv in attackerShips)
//        {
//            if (kv.Key.canAttack)
//            {
//                for (int i = 0; i < kv.Value; i++)
//                    attackers.Add(kv.Key);
//            }
//        }

//        List<ShipType> defenders = new List<ShipType>();
//        foreach (var kv in defenderShips)
//        {
//            for (int i = 0; i < kv.Value; i++)
//                defenders.Add(kv.Key);
//        }

//        // Battle loop
//        while (attackers.Count > 0 && defenders.Count > 0)
//        {
            
//            List<ShipType> currentAttackers = new List<ShipType>(attackers);

//            foreach (var attacker in currentAttackers)
//            {
//                if (defenders.Count == 0) break;

//                // Pick a random defender
//                int defenderIndex = Random.Range(0, defenders.Count);
//                ShipType defender = defenders[defenderIndex];

//                // Roll
//                int attackerRoll = Random.Range(1, maxRoll*attacker.attackPower + 1);
//                int defenderRoll = Random.Range(1, maxRoll*defender.attackPower + 1);

//                if (attackerRoll > defenderRoll)
//                {   
//                    defenders.RemoveAt(defenderIndex);
//                }
//                else
//                {
//                    attackers.Remove(attacker);
//                }

//                // End battle if one side is empty
//                if (attackers.Count == 0 || defenders.Count == 0) break;
//            }
//        }

//        // Count remaining ships by type for attacker and defender
//        int remainingAttackers = attackers.Count;
//        int remainingDefenders = defenders.Count;

//        attackerWon = remainingAttackers > 0;

//        Debug.Log($"Battle Result: Attacker Ships Remaining: {remainingAttackers}, Defender Ships Remaining: {remainingDefenders}, Attacker Won: {attackerWon}");
//        return new BattleResult(remainingAttackers, remainingDefenders, attackerWon);
//    }

//    public BattleResult BattleBrianVersion(Dictionary<ShipType, int> attackerShips, Dictionary<ShipType, int> defenderShips)
//    {
//        int attackerShipPower = 0;
//        int defenderShipPower = 0;

//        foreach(var ship in attackerShips)
//        {
//            if (ship.Key.canAttack == true)
//            {
//                attackerShipPower += ship.Value * ship.Key.attackPower;
//            }
//        }

//        foreach (var ship in defenderShips)
//        {
//            defenderShipPower += ship.Value * ship.Key.attackPower;
//        }

//        float chanceForSuccess = (float)attackerShipPower / (attackerShipPower + defenderShipPower);

//        bool attackerWon = Random.value < chanceForSuccess;

//        Debug.Log($"Attacker Won: {attackerWon} at {chanceForSuccess*100}%");
//        return new BattleResult(0, 0, attackerWon);
//    }
//}
