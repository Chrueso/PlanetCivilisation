
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle")]
public class AIIdleAction : AIAction
{
    public override void Execute(AIContext context)
    {
        EntityModel model = context.Model;
        model.RemoveAP(1);
    }
}
