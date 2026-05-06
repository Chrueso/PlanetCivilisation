using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuView : ScreenBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text PlanetName;
    [SerializeField] private InfoElements Faction;
    [SerializeField] private InfoElements Abundant;
    [SerializeField] private InfoElements AbundantGain;
    [SerializeField] private InfoElements Scarce;
    [SerializeField] private InfoElements ScarceGain;
    [SerializeField] private InfoElements Structures;
    [SerializeField] private InfoElements Ships;
    private List<InfoElements> infoElementList;
    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseView);
        infoElementList = new List<InfoElements>() { 
            Faction, Abundant, Scarce, ScarceGain, Structures, Ships
        };
    }

    public void UpdateInfo(IGridHexObject hexOccupant)
    {
        if (hexOccupant is PlanetData planetData)
        {
            string structures = planetData.Structures.Count > 0
                ? string.Join(", ", planetData.Structures)
                : "None";

            string ships = planetData.StationedShips.Count > 0
                ? string.Join("\n", planetData.StationedShips
                    .Where(kvp => kvp.Value > 0)
                    .Select(kvp => $"  {kvp.Key}: {kvp.Value}"))
                : "  None";

            PlanetName.text = planetData.PlanetName;
            Faction.ChangeText("Faction:", planetData.FactionType.ToString());

            Abundant.ChangeText("Abundant:", planetData.PlanetResource[ResourceClass.Abundant].ToString());
            AbundantGain.ChangeText("Abundant/turn:", $"{planetData.GeneratedResource[planetData.PlanetResource[ResourceClass.Abundant]]}/turn");
            Scarce.ChangeText("Abundant:", planetData.PlanetResource[ResourceClass.Scarce].ToString());
            ScarceGain.ChangeText("Abundant/turn:", $"{planetData.GeneratedResource[planetData.PlanetResource[ResourceClass.Scarce]]}/turn");
            Structures.ChangeText("Structures:", structures);
            Ships.ChangeText("Ships:", ships);
            ElementVisibility(true);
        }
        else
        {
            PlanetName.text = "None";
            ElementVisibility(false);
        }
    }
    private void ElementVisibility(bool visible)
    {
        if (visible)
        {
            foreach (var element in infoElementList)
            {
                element.Show();
            }
        }
        else
        {
            foreach (var element in infoElementList)
            {
                element.Omit();
            }
        }
    }
    private void CloseView()
    {
        AudioService.CurrentAudioInstance.PlayOneShot(GameManager.audioLib.general, 0.5f);
        GameScreenManager.Pop();
    }
}
