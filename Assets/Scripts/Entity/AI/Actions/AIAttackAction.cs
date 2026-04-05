using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AIAttackAction : AIAction
{
    [SerializeField] private AnimationCurve attackCurve;

    public override float CalculateUtility(AIContext context)
    {
        if (context.IsOnEnemyPlanet)
        {
            int planetDefense = context.CurrentPlanet.CalculateDefensePower();
            return planetDefense > 0 ? Mathf.Clamp01(attackCurve.Evaluate((float)context.AttackPower / planetDefense)) : 1f;
        }
        else
        {
            return 0f;
        }
    }

    public override void Execute(AIContext context)
    {
        context.Controller.TryAttack(context.CurrentPlanet);
    }
}
