using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : ScreenBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private InfoElements faction;
    [SerializeField] private InfoElements planetsOwned;
    [SerializeField] private InfoElements planetsDiscovered;
    [SerializeField] private InfoElements assault;
    [SerializeField] private InfoElements worker;
    [SerializeField] private InfoElements scout;
    [SerializeField] private InfoElements mpt;
    [SerializeField] private InfoElements rpt; 
    private EntityModel playerInfo;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseView);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseView);
    }

    public void SetPlayer(EntityModel playerInfo)
    {
        this.playerInfo = playerInfo;
    }

    private void CloseView()
    {
        GameScreenManager.Pop();
    }

    protected override void OnShow()
    {
        faction.ChangeText("Faction", playerInfo.FactionType.ToString());
        planetsOwned.ChangeText("Planets Owned:", playerInfo.OwnedPlanets.Count.ToString());
        planetsDiscovered.ChangeText("Planets Discovered:", playerInfo.DiscoveredPlanets.Count.ToString());
        assault.ChangeText("Assault Ships:", playerInfo.Ships.ContainsKey(ShipType.Attacker) ? playerInfo.Ships[ShipType.Attacker].ToString() : "0");
        worker.ChangeText("Assault Ships:", playerInfo.Ships.ContainsKey(ShipType.Worker) ? playerInfo.Ships[ShipType.Worker].ToString() : "0");
        scout.ChangeText("Assault Ships:", playerInfo.Ships.ContainsKey(ShipType.Scout) ? playerInfo.Ships[ShipType.Scout].ToString() : "0");
        int totalMetalGain = 0;
        int totalRationGain = 0;
        foreach (var planet in playerInfo.OwnedPlanets)
        {
            totalMetalGain += planet.GeneratedResource[ResourceType.Metals];
            totalRationGain += planet.GeneratedResource[ResourceType.Rations];
        }
        mpt.ChangeText("Metals/turn:", totalMetalGain.ToString());
        rpt.ChangeText("Rations/turn:", totalRationGain.ToString());
        
    }
}
