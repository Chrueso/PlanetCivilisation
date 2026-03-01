using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
public class Crafter : Singleton<Crafter>
{
    [SerializeField] private Button craftExtractor;
    [SerializeField] private Button craftShipyard;

    //recipes
    private Dictionary<Structure, Dictionary<Resource, int>> Recipe = new Dictionary<Structure, Dictionary<Resource, int>>() {
        {Structure.Extractor, new Dictionary<Resource, int>() { { Resource.Metals, 1}, { Resource.Rations, 1 }, { Resource.Energy_Source, 1 } } },
        {Structure.Shipyard, new Dictionary<Resource, int>() { { Resource.Metals, 1}, { Resource.Rations, 1 }, { Resource.Energy_Source, 0 } } },
    };
    //hardcode for now
    private Dictionary<Resource, int> PlayerInv = new Dictionary<Resource, int>() {
        {Resource.Metals, 5 },
        {Resource.Rations, 5 },
        {Resource.Energy_Source, 5 },
    };
    private void OnEnable()
    {
        craftExtractor.onClick.AddListener(CraftExtractor);
        craftShipyard.onClick.AddListener(CraftShipyard);
    }

    private void CraftExtractor()
    {
        if (CheckPlayerInv(Structure.Extractor))
        {
            PlayerInteractionController.Instance.CurrentPlanet.DebugPurposes();
            UINavigationManager.Instance.BackFromOverlay();
        }
    }

    private void CraftShipyard()
    {
        if (CheckPlayerInv(Structure.Shipyard))
        {
            PlayerInteractionController.Instance.CurrentPlanet.DebugPurposes();
            UINavigationManager.Instance.BackFromOverlay();
        }
    }

    private bool CheckPlayerInv(Structure structure)
    {
        bool canCraft = false;
        foreach (KeyValuePair<Resource, int> recipe in Recipe[structure] )
        {
            if (PlayerInv[recipe.Key] > recipe.Value) {
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
