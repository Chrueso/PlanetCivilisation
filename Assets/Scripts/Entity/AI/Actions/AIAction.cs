using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public virtual void Init(AIContext context)
    {
        // Optional setup
    }

    public virtual float CalculateUtility(AIContext context)
    {
        return 0;
    }

    public abstract void Execute(AIContext context);    
}
