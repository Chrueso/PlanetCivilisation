using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class HUDView : ScreenBase
{
    public override bool ShouldHonorBackButton => false;
    public override bool ShouldUnfocusPrevScreen => false;

    public Button SettingsButton;
    public Button PlanetListButton;
    public Button HomeShipButton;
    public Button EndTurnButton;

    public TMP_Text CurrentTurnText;

    public FactionWidget FactionWidget;

    public List<ResourcesWidget> ResourcesWidgets;

    public void UpdateFaction(FactionType faction)
    {

    }

    public void UpdateResources(Dictionary<ResourceType, int> resources)
    {
        
    }

}
