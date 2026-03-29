using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AIAttackAction : AIAction
{
    [SerializeField] private AnimationCurve attackCurve;

    public override void Init(AIContext context)
    {
        attackCurve = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 1));
    }

    public override float CalculateUtility(AIContext context)
    {
        if (context.IsOnEnemyPlanet)
        {
            int planetDefense = context.CurrentPlanet.CalculateDefensePower();
            return planetDefense > 0 ? attackCurve.Evaluate((float)context.AttackPower / planetDefense) : 1f;
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
