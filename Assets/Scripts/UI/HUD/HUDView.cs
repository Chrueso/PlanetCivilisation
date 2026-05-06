using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class HUDView : ScreenBase
{
    public override bool ShouldShowScreenRaycastBlocker => false;
    public override bool ShouldHonorBackButton => false;

    public Button SettingsButton;
    public Button PlanetListButton;
    public Button HomeShipButton;
    public Button EndTurnButton;
    public Button InfoButton;

    public Button ZoomIn;
    public Button ZoomOut;

    [SerializeField] public TextMeshProUGUI CurrTurnText;

    //public TMP_Text CurrentTurnText;
    //public FactionWidget FactionWidget;

    [SerializeField] private List<ResourcesWidget> resourcesWidgets = new List<ResourcesWidget>();
    private Dictionary<ResourceType, ResourcesWidget> resourceWidgetDict = new Dictionary<ResourceType, ResourcesWidget>();

    public TMP_Text CurrentAPText;

    public Button NextEntityButton;

    public void UpdateAP(int currentAP, int maxAP)
    {
        CurrentAPText.text = $"{currentAP}/{maxAP} AP";
    }

    public void UpdateTurnsSignal(FactionType currentTurn)
    {
        if (currentTurn == FactionType.Human)
        {
            StartCoroutine(ShowText());
        }
        else
        {
            string displayText = $"CURRENT TURN: {currentTurn}\n(CURRENTLY NOT YOUR TURN)";
            CurrTurnText.text = displayText;
            CurrTurnText.enabled = true;
        }
    }
    private IEnumerator ShowText()
    {

        string displayText = $"CURRENT TURN: {FactionType.Human}\n(YOUR TURN)";
        CurrTurnText.text = displayText;
        CurrTurnText.enabled = true;

        yield return new WaitForSeconds(1f);       
        CurrTurnText.enabled = false;

    }
    public void HandleResources()
    {
        foreach (var widget in resourcesWidgets)
        {
            resourceWidgetDict.Add(widget.ResourceType, widget);
            widget.AmountText.text = "";
        }
    }

    public void UpdateFaction(FactionType faction)
    {

    }

    public void UpdateResources(Dictionary<ResourceType, int> resources)
    {
        foreach(var widget in resourcesWidgets)
        {
            widget.AmountText.text = "";
        }

        foreach (var kvp in resources)
        {
            resourceWidgetDict[kvp.Key].AmountText.text = kvp.Value.ToString();
        }
    }

    public void EnableDebug()
    {
        NextEntityButton.gameObject.SetActive(true);
    }

    public void DisableDebug()
    {
        NextEntityButton.gameObject.SetActive(false);
    }

}
