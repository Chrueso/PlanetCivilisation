using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle")]
public class AIIdleAction : AIAction
{
    public override void Execute(AIContext context)
    {
        Debug.Log("Idling");
    }
}
