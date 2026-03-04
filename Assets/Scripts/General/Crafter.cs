using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Crafter : Singleton<Crafter>
{
    [SerializeField] private Button craftExtractor;
    [SerializeField] private Button craftShipyard;

    //recipes
    private Dictionary<Structure, Dictionary<ResourceType, int>> Recipe = new Dictionary<Structure, Dictionary<ResourceType, int>>() {
        {Structure.Extractor, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Energy_Source, 1 } } },
        {Structure.Shipyard, new Dictionary<ResourceType, int>() { { ResourceType.Metals, 1}, { ResourceType.Rations, 1 }, { ResourceType.Energy_Source, 0 } } },
    };
    //hardcode for now
    private Dictionary<Resource, int> PlayerInv = new Dictionary<Resource, int>() {
        {Resource.Metals, 5 },
        {Resource.Rations, 5 },
        {Resource.Energy_Source, 5 },
    };
    private void OnEnable()
    {
        craftExtractor.onClick.AddListener(delegate { BuildStructure(Structure.Extractor); });
        craftShipyard.onClick.AddListener(delegate { BuildStructure(Structure.Shipyard); });
    }

    private void BuildStructure(Structure structure)
    {
        if (!TurnManager.Instance.currentFaction.DecreaseTurn(1)) return;
        if (CheckPlayerInv(structure))
        {
            EventsHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU SUCCESSFULLY BUILT STRUCTURE");
            PlayerInteractionController.Instance.CurrentPlanet.BuildStructure(structure);
            UINavigationManager.Instance.BackFromOverlay();
        } else
        {
            EventsHandler.Instance.RunSimulationScreen("BUILD PROCESSING", $"YOU ARE BUILDING {structure}", "BUILD OUTCOME", "YOU FAILED TO BUILT STRUCTURE");
        }
        
        UINavigationManager.Instance.UpdateFriendlyUI();
    }

    private bool CheckPlayerInv(Structure structure)
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
