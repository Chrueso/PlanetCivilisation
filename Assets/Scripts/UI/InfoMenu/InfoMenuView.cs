using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuView : ScreenBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text infoText;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseView);
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

            infoText.text =
                $"Planet: {planetData.PlanetName}\n" +
                $"Faction: {planetData.FactionType}\n" +
                $"\nResources\n" +
                $"  Abundant: {planetData.PlanetResource[ResourceClass.Abundant]}\n" +
                $"  Scarce:   {planetData.PlanetResource[ResourceClass.Scarce]}\n" +
                $"\nStructures: {structures}\n" +
                $"\nStationed Ships\n{ships}";
        }
        else
        {
            infoText.text = "No information available.";
        }
    }

    private void CloseView()
    {
        GameScreenManager.Pop();
    }
}
