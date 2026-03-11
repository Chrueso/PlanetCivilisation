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