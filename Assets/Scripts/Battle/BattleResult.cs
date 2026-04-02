using System.Collections.Generic;

public class BattleResult
{
    public int AttackerShipsRemaining { get; private set; }
    public int DefenderShipsRemaining { get; private set; }
    public bool AttackerWon { get; private set; }
    public List<BattleStep> BattleSteps { get; private set; }

    public BattleResult(int attackerShipsRemaining, int defenderShipsRemaining, bool attackerWon, List<BattleStep> battleSteps)
    {
        AttackerShipsRemaining = attackerShipsRemaining;
        DefenderShipsRemaining = defenderShipsRemaining;
        AttackerWon = attackerWon;
        BattleSteps = battleSteps;
    }
}

public class BattleStep
{
    public ShipType attackerType { get; private set; }
    public ShipType defenderType { get; private set; }
    public bool attackerWon { get; private set; }

    public BattleStep(ShipType attackerType, ShipType defenderType, bool attackerWon)
    {
        this.attackerType = attackerType;
        this.defenderType = defenderType;
        this.attackerWon = attackerWon;
    }
}