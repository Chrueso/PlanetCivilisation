using System.Collections.Generic;
using UnityEngine;

public class AIBrain 
{
    private List<AIAction> actions = new List<AIAction>();
    private AIContext context;

    private EntityController controller => context.Controller;
    private int currentAP => context.Model.CurrentAP;
    private bool isCurrentTurn => context.Controller.IsCurrentTurn;

    private EventBinding<TurnChangeEvent> turnChangeEventBinding;
   
    public AIBrain(EntityController controller, List<AIAction> actions)
    {
        this.actions =  actions;

        context = new AIContext(this, controller.GetModel(), controller);

        foreach (var action in actions)
        {
            action.Init(context);
        }

        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(HandleTurnChange);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
    }

    private void HandleTurnChange(TurnChangeEvent turnChangeEvent)
    {
        if (isCurrentTurn) Think();
    }

    public void Think()
    {
        while (currentAP > 0)
        {
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
                bestAction.Execute(context);
            }
        }

        // No ap left
        controller.TryEndTurn(); 
    }
}
