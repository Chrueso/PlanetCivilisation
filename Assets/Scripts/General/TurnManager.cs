using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] UINavigationManager uINavigationManager;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnDisplay;
    [SerializeField] private TextMeshProUGUI metalsDisplay;
    [SerializeField] private TextMeshProUGUI rationsDisplay;
    [SerializeField] private TextMeshProUGUI energyDisplay;
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
        endTurnButton.onClick.AddListener(EndTurn);
        StartCoroutine(wtf());
    }

    private IEnumerator wtf()
    {
        yield return new WaitForSeconds(1);
        UpdateResourceVisuals();
    }

    public void StartTurn()
    {
        if (currentFaction.FactionType == FactionType.Human)
        {
            turnDisplay.text = "HUMAN";
        }
        else if (currentFaction.FactionType == FactionType.DemiHuman)
        {
            turnDisplay.text = "DEMIHUMAN";
        } else if (currentFaction.FactionType == FactionType.IntelligentConstruct)
        {
            turnDisplay.text = "INTELLIGENT CONSTRUCT";
        }
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
            EndPlayerTurn();
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
        if (playerTurn)
        {
            uINavigationManager.SetHomeShipButton(false);
            GameManager.Instance.Player.CalculateResourceGain();
            UpdateResourceVisuals();
            if (GameManager.Instance.Player.OwnedPlanets.Count >= 5 && GameManager.Instance.Player.Resources[ResourceType.Metals] > 30)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void UpdateResourceVisuals()
    {
        metalsDisplay.text = $"METALS: {GameManager.Instance.Player.Resources[ResourceType.Metals]}";
        rationsDisplay.text = $"RATIONS: {GameManager.Instance.Player.Resources[ResourceType.Rations]}";
        energyDisplay.text = $"ENERGY: {GameManager.Instance.Player.Resources[ResourceType.Credits]}";
    }

}
