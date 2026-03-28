using System.Collections.Generic;
using UnityEngine;

public class AIBrain
{
    private List<AIAction> actions = new List<AIAction>();
    private AIContext context;

    private EntityController controller;
    private EntityModel model;

    private int currentAP => model.CurrentAP;

    public AIBrain(EntityController controller, List<AIAction> actions)
    {
        this.actions = actions;
        this.controller = controller;
        this.model = controller.GetModel();

        context = new AIContext(this, model, controller);

        foreach (var action in actions)
        {
            action.Init(context);
        }

        controller.OnCurrentTurn += Think;
    }

    public void Think()
    {
        int safetyLimit = 100;
        while (currentAP > 0 && safetyLimit-- > 0)
        {
            Debug.Log(context.Model.FactionType + " started thinking...");

            AIAction bestAction = null;
            float highestUtility = float.MinValue;

            foreach (var action in actions)
            {
                float utility = action.CalculateUtility(context);

                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                Debug.Log(context.Model.FactionType + " Best Action: " + bestAction.ToString() + " | Utility: " + highestUtility.ToString());
                bestAction.Execute(context);
            }
        }

        if (safetyLimit <= 0)
            Debug.LogError(context.Model.FactionType + " Think() hit safety limit — possible infinite loop.");

        // No ap left
        controller.TryEndTurn();
    }
}
