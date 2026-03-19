using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManager
{
    public bool playerTurn { get { return currentFaction.IsPlayer; } } 
    private int currentFactionIndex = 0;

    private FactionManager factionManager;
    public Faction currentFaction { get { return factionManager.FactionsInUse[currentFactionIndex]; } }

    public event System.Action OnTurnEnd;
    public event System.Action OnTurnStart;

    public TurnManager(FactionManager factionManager)
    {
        this.factionManager = factionManager;
    }

    //make something call this at the start of the game
    public void StartTurn()
    {

        currentFaction.BeginTurn(this);
        currentFaction.OnTurnFinished += EndTurn;

        OnTurnStart?.Invoke();

        if (currentFaction.ActionPoints > 0)
        {
            Debug.Log($"Starting turn for {currentFaction.FactionType}");
            if (!playerTurn)
            {
               RunAiTurn();
            }
            else
            {
               StartPlayerTurn();
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log($"Ending turn for {currentFaction.FactionType}");

        currentFaction.OnTurnFinished -= EndTurn;
        OnTurnEnd?.Invoke();

        if (playerTurn)
        {
            EndPlayerTurn();
        }
        else
        {
            //do Faction stuff here ig
            
        }
        currentFactionIndex = (currentFactionIndex + 1) % factionManager.FactionsInUse.Count;
        StartTurn();
    }

    private void RunAiTurn()
    {
        while (currentFaction.ActionPoints > 0)
        {
            currentFaction.RunAIAction();
        }

        currentFaction.FinishTurn();
    }

    public void StartPlayerTurn()
    {

    }

    public void EndPlayerTurn()
    {
        if (playerTurn)
        {
            //GameManager.Instance.Player.CalculateResourceGain();
            //if (GameManager.Instance.Player.OwnedPlanets.Count >= 5 && GameManager.Instance.Player.Resources[ResourceType.Metals] > 30)
            //{
              //  SceneManager.LoadScene("MainMenu");
            //}
        }
    }

}
