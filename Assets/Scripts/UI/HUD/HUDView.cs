using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HUDView : ScreenBase
{
    public override bool ShouldShowScreenRaycastBlocker => false;
    public override bool ShouldHonorBackButton => false;

    public Button SettingsButton;
    public Button PlanetListButton;
    public Button HomeShipButton;
    public Button EndTurnButton;

    public TMP_Text CurrentTurnText;

    public FactionWidget FactionWidget;

    //private List<Sprite> resourceIcons;
    //private ResourcesWidget resourceWidgetPrefab;

    [SerializeField] private List<ResourcesWidget> resourcesWidgets = new List<ResourcesWidget>();
    private Dictionary<ResourceType, ResourcesWidget> resourceWidgetDict = new Dictionary<ResourceType, ResourcesWidget>();

    public Button NextEntityButton;

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
