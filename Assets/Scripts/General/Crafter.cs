using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Crafter : Singleton<Crafter>
{
    [SerializeField] UINavigationManager uINavigationManager;
    [SerializeField] private Button craftExtractor;
    [SerializeField] private Button craftShipyard;

    //recipes
    private Dictionary<StructureType, Dictionary<ResourceType, int>> Recipe = new Dictionary<StructureType, Dictionary<ResourceType, int>>() {
        {StructureType.Extractor, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Energy_Source, 1 } } },
        {StructureType.Shipyard, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Energy_Source, 0 } } },
    };

    //hardcode for now
    private Dictionary<ResourceType, int> PlayerInv = new Dictionary<ResourceType, int>() {
        {ResourceType.Metals, 5 },
        {ResourceType.Rations, 5 },
        {ResourceType.Energy_Source, 5 },
    };
    private void OnEnable()
    {
        craftExtractor.onClick.AddListener(delegate { BuildStructure(StructureType.Extractor); });
        craftShipyard.onClick.AddListener(delegate { BuildStructure(StructureType.Shipyard); });
    }

    private void BuildStructure(StructureType structure)
    {
        if (!TurnManager.Instance.currentFaction.DecreaseActionPoint(1)) return;
        if (CheckPlayerInv(structure))
        {
            SimulationHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU SUCCESSFULLY BUILT STRUCTURE");
            PlayerInteractionController.Instance.CurrentPlanet.BuildStructure(structure);
            uINavigationManager.BackFromOverlay();
        } else
        {
            SimulationHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU FAILED TO BUILT STRUCTURE");
        }

        uINavigationManager.UpdateFriendlyUI();
    }

    private bool CheckPlayerInv(StructureType structure)
    {
        bool canCraft = false;
        foreach (KeyValuePair<ResourceType, int> recipe in Recipe[structure] )
        {
            if (GameManager.Instance.Player.Resources[recipe.Key] > recipe.Value) {
                canCraft = true;
            } else
            {
                canCraft = false;
                break;
            }
        }
        return canCraft;
    }

    // need structure data too
    // missing data but like its just if has enough of x then yes craft it
    // basically should send a signal to say this planet does indeed have a structure
}
