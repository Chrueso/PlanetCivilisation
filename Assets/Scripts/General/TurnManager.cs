using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : Singleton<TurnManager>
{
    public bool playerTurn { get { return currentFaction.IsPlayer; } } 
    private int currentFactionIndex = 0;
    public Faction currentFaction { get { return FactionManager.Instance.FactionsInUse[currentFactionIndex]; } }


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        currentFaction.BeginTurn();
        currentFaction.OnTurnFinished += EndTurn;

        if (currentFaction.ActionPoints > 0)
        {

            Debug.Log($"Starting turn for {currentFaction.FactionType}");
            if (!playerTurn)
            {
               StartCoroutine(RunAiTurn());
            }
            else
            {
                //currentFaction.StartPlayerTurn();
                //player stuff

                StartPlayerTurn();

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

    private IEnumerator RunAiTurn()
    {
        while (currentFaction.ActionPoints > 0)
        {
            currentFaction.RunAIAction();
            yield return new WaitForSeconds(0.3f);
        }

        currentFaction.FinishTurn();
    }

    public void StartPlayerTurn()
    {
        //enable UI and stuff
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    EndTurn();
        //}
    }

    public void EndPlayerTurn()
    {
        if(playerTurn) EndTurn();
    }

}
