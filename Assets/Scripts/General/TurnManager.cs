using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool playerTurn { get; private set; } = true;

    [SerializeField] private FactionDataSO[] factionDatas;

    private Faction[] factions;
    protected override void Awake()
    {
        base.Awake();

        //makes new factions fromm the faction data
        factions = new Faction[factionDatas.Length];
        for (int i = 0; i < factionDatas.Length; i++)
        {
            factions[i] = new Faction(factionDatas[i]);
        }

        StartPlayerTurn();
    }
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

    // does all the other world/ Ai faction actions here
    public void ResolveWorldTurn()
    {
        foreach (var faction in factions)
        {
            faction.StartWorldTurn();
        }   
    }
}
