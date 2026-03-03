using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EventsHandler : Singleton<EventsHandler>
{
    public void RunBattleSimulation()
    {
        StartCoroutine(VisualHardcode());
        
    }

    public void RunDiplomacySimulation()
    {
        StartCoroutine(VisualHardcode2());
    }

    public void RunCraftingSimulation()
    {
        StartCoroutine(VisualHardcode3());
    }

    public void RunStationingSimulation()
    {
        StartCoroutine(VisualHardcode4());
    }

    public void RunSimulationScreen(string upperText1, string lowerText1, string upperText2, string lowerText2)
    {
        StartCoroutine(Visualizer(upperText1, lowerText1, upperText2, lowerText2));
    }
    private IEnumerator Visualizer(string upperText, string lowerText, string upperText2, string lowerText2)
    {
        StatusPanel.Instance.ShowText(upperText, lowerText);
        yield return new WaitForSeconds(2);
        StatusPanel.Instance.ShowText(upperText2, lowerText2);
        yield return new WaitForSeconds(2);
    }
    private IEnumerator VisualHardcode4()
    {
        StatusPanel.Instance.ShowText("STATIONING HAPPENING", $"SENDING YOUR SHIPS");
        yield return new WaitForSeconds(2);
        StatusPanel.Instance.ShowText("STATIONING OUTCOME", $"SHIPS ARE STATIONED");
        yield return new WaitForSeconds(2);
    }

    private IEnumerator VisualHardcode3()
    {
        StatusPanel.Instance.ShowText("CRAFTING HAPPENING", $"CHECKING YOUR RESOURCES");
        yield return new WaitForSeconds(2);
        StatusPanel.Instance.ShowText("CRAFTING OUTCOME", $"YOU HAVE ENOUGH, HERE YOU GO");
        yield return new WaitForSeconds(2);
    }

    private IEnumerator VisualHardcode2()
    {
        StatusPanel.Instance.ShowText("DIPLOMACY HAPPENING", $"YOU ARE NEGOTIATING");
        yield return new WaitForSeconds(2);
        StatusPanel.Instance.ShowText("DIPLOMACY OUTCOME", $"HELL NAH GO AWAY");
        yield return new WaitForSeconds(2);
    }

    private IEnumerator VisualHardcode()
    {
        StatusPanel.Instance.ShowText("ATTACK HAPPENING", $"ATTACKER IS ATTACKING DEFENDER");
        yield return new WaitForSeconds(2);
        StatusPanel.Instance.ShowText("ATTACK OUTCOME", $"PERSON WON");
        yield return new WaitForSeconds(2);
    }
}
