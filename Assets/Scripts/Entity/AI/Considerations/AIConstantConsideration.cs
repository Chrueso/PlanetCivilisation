using UnityEngine;

[CreateAssetMenu(menuName = "AI/Considerations/Constant")]
public class AIConstantConsideration : AIConsideration
{
    public float Value;

    public override float Evaluate(AIContext context) => Value;
}
