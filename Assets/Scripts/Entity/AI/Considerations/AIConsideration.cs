using UnityEngine;

public abstract class AIConsideration : ScriptableObject
{
    public abstract float Evaluate(AIContext context);
}
