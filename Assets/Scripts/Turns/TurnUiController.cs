using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUiController : MonoBehaviour
{
    //[SerializeField] UINavigationManager uINavigationManager;
    //[SerializeField] private Button endTurnButton;
    //[SerializeField] private TextMeshProUGUI turnDisplay;
    //[SerializeField] private TextMeshProUGUI metalsDisplay;
    //[SerializeField] private TextMeshProUGUI rationsDisplay;
    //[SerializeField] private TextMeshProUGUI energyDisplay;

    private TurnManager turnManager;

    void Awake()
    {
       
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.OnTurnStart -= HandleTurnStartUI;
            turnManager.OnTurnEnd -= HandleTurnEndUI;

        }
    }

    private void Start()
    {
        turnManager = GameManager.Instance.turnManager;
        turnManager.OnTurnStart += HandleTurnStartUI;
        turnManager.OnTurnEnd += HandleTurnEndUI;

        //if (endTurnButton != null)
        //{
        //    endTurnButton.onClick.AddListener(turnManager.EndTurn);
        //}
    }

    private void HandleTurnStartUI()
    {
        StartCoroutine(UpdateResourceTimer());
        idk();
    }

    private void HandleTurnEndUI()
    {
        StartCoroutine(UpdateResourceTimer());
        //uINavigationManager.SetHomeShipButton(false);
    }

    private IEnumerator UpdateResourceTimer()
    {
        yield return new WaitForSeconds(1.0f);
        UpdateResourceVisuals();
    }

    //js get rid of ts once the ui is fixed
    private void UpdateResourceVisuals()
    {
        //metalsDisplay.text = $"METALS: {GameManager.Instance.Player.Resources[ResourceType.Metals]}";
        //rationsDisplay.text = $"RATIONS: {GameManager.Instance.Player.Resources[ResourceType.Rations]}";
        //energyDisplay.text = $"ENERGY: {GameManager.Instance.Player.Resources[ResourceType.Credits]}";
    }

    //js get rid of ts once the ui is fixed
    private void idk()
    {
        //switch(turnManager.currentFaction.FactionType)
        //{
        //    case FactionType.Human:
        //        turnDisplay.text = "HUMAN";
        //        break;
        //    case FactionType.DemiHuman:
        //        turnDisplay.text = "DEMIHUMAN";
        //        break;
        //    case FactionType.IntelligentConstruct:
        //        turnDisplay.text = "INTELLIGENT CONSTRUCT";
        //        break;
        //}
    }


}
