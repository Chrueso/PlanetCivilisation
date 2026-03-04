using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DiplomacySystem : Singleton<DiplomacySystem>
{
    [SerializeField] private Button DiplomacyButton;
    // hardcoded values cause dont have data yet
    private Dictionary<FactionType, int> demihumanRelationships = new Dictionary<FactionType, int>() { {FactionType.Human, 20 }, { FactionType.IntelligentConstruct, 100 } };
    private FactionType playerFaction = FactionType.Human;
    private int playerHomeShipAttackerAmt = 2;
    private int demihumanHomeShipAttackerAmt = 3;
    private float demihumanHostility = 0.5f;
    private void OnEnable()
    {
        DiplomacyButton.onClick.AddListener(TryForAlliance);
    }

    private void TryForAlliance()
    {
        if (!TurnManager.Instance.currentFaction.DecreaseTurn(1)) return;
        bool success = false;
        PlanetData pData = PlayerInteractionController.Instance.CurrentPlanet;
        if (pData.Affection.TryGetValue(playerFaction, out int affinity))
        {
            if (affinity < 40)
            {
                print("NO I KILL U");
                success = false;
            } 
            else
            {
                print("YES");
                success = true;
            }
        }

        float attackWeightage = Mathf.Clamp01( ( (demihumanHomeShipAttackerAmt - playerHomeShipAttackerAmt) / (demihumanHomeShipAttackerAmt+playerHomeShipAttackerAmt) ) + demihumanHostility);
        string words;
        if (success)
        {
            words = "SUCCESS";
            pData.SetFaction(playerFaction);
            GameManager.Instance.Player.AddOwnedPlanets(pData);
        }
        else
        {
            words = "FAIL";
        }

        UINavigationManager.Instance.DismissAllSheets();
        CameraController.Instance.Enable();
        EventsHandler.Instance.RunSimulationScreen("DIPLOMACY HAPPENING", "YOU ARE TRYING TO DIPLOMACY THIS PLANET", "DIPLOMACY OUTCOME", $"{words}");
    }
}
