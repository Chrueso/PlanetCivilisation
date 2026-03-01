using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public bool playerTurn { get { return currentFaction.IsPlayer; } } 
    private int currentFactionIndex = 0;
    public Faction currentFaction { get { return FactionManager.Instance.FactionsInUse[currentFactionIndex]; } }


    protected override void Awake()
    {
        base.Awake();

        StartTurn();
    }

    public void StartTurn()
    {
        currentFaction.OnTurnFinished += EndTurn;

        if (currentFaction.ActionPoints > 0)
        {

            Debug.Log($"Starting turn for {currentFaction.FactionType}");
            if (!playerTurn)
            {
                currentFaction.StartAITurn();
            }
            else
            {
                currentFaction.StartPlayerTurn();
                //player stuff
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log($"Ending turn for {currentFaction.FactionType}");

        currentFaction.OnTurnFinished -= EndTurn;

        if (playerTurn)
        {
            //EndPlayerTurn();
        }
        else
        {
            //do Faction stuff here ig
            
        }
        currentFactionIndex = (currentFactionIndex + 1) % FactionManager.Instance.FactionsInUse.Count;
        StartTurn();
    }

    public void StartPlayerTurn()
    {
        //enable UI and stuff
    }
}
