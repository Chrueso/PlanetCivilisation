using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool playerTurn { get; private set; } = true;
    [SerializeField] private Faction[] factions;

    public void EndPlayerTurn()
    {
        playerTurn = false;
        Debug.Log("Player turn ended. Now it's AI's turn.");

        ResolveWorldTurn();

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        playerTurn = true;
        Debug.Log("AI turn ended. Now it's Player's turn.");
    }

    public void ResolveWorldTurn()
    {
        foreach (var faction in factions)
        {
            faction.StartWorldTurn();
        }   
    }
}
