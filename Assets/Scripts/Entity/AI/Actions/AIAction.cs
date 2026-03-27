using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public AIConsideration Consideration;

    public virtual void Init(AIContext context)
    {
        // Optional setup
    }

    public float CalculateUtility(AIContext context) => Consideration.Evaluate(context);

    public abstract void Execute(AIContext context);    
}
