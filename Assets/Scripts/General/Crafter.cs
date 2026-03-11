using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Crafter : MonoBehaviour
{
    [SerializeField] UINavigationManager uINavigationManager;
    [SerializeField] private Button craftExtractor;
    [SerializeField] private Button craftShipyard;

    //recipes
    private Dictionary<Structure, Dictionary<ResourceType, int>> Recipe = new Dictionary<Structure, Dictionary<ResourceType, int>>() {
        {Structure.Extractor, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Credits, 1 } } },
        {Structure.Shipyard, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Credits, 0 } } },
    };
   
    private void OnEnable()
    {
        craftExtractor.onClick.AddListener(delegate { BuildStructure(StructureType.Extractor); });
        craftShipyard.onClick.AddListener(delegate { BuildStructure(StructureType.Shipyard); });
    }

    private void OnDisable()
    {
        craftExtractor.onClick.RemoveListener(delegate { BuildStructure(Structure.Extractor); });
        craftShipyard.onClick.RemoveListener(delegate { BuildStructure(Structure.Shipyard); });
    }

    private void BuildStructure(Structure structure)
    {
        // if (!TurnManager.Instance.currentFaction.DecreaseTurn(1)) return;  should get player turn from GameManager
        /*
        if (CheckPlayerInv(structure))
        {
            SimulationHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU SUCCESSFULLY BUILT STRUCTURE");
            PlayerInteractionController.Instance.CurrentPlanet.BuildStructure(structure);
            uINavigationManager.BackFromOverlay();
        } else
        {
            SimulationHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU FAILED TO BUILT STRUCTURE");
        }
        
        UINavigationManager.Instance.UpdateFriendlyUI();
        */
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
