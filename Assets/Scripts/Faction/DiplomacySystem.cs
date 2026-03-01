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
        if (demihumanRelationships.TryGetValue(playerFaction, out int affinity))
        {
            if (affinity < 40)
            {
                print("NO I KILL U");
            } 
            else if (affinity > 40 && affinity < 80)
            {
                print("HMM MAYBE");
            } 
            else if (affinity > 80)
            {
                print("YES");
            }
        }

        float attackWeightage = Mathf.Clamp01( ( (demihumanHomeShipAttackerAmt - playerHomeShipAttackerAmt) / (demihumanHomeShipAttackerAmt+playerHomeShipAttackerAmt) ) + demihumanHostility);
        print(attackWeightage);
        if (attackWeightage > 0.5f)
        {
            print("ATTACK");
        } else
        {
            print("MEH GO AWAY");
        }

        UINavigationManager.Instance.BackFromOverlay();
    }
}
