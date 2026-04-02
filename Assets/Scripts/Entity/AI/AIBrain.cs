using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

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

        context = new AIContext(this, controller);

        foreach (var action in actions)
        {
            action.Init(context);
        }

        controller.OnCurrentTurn += HandleOnCurrentTurn;
    }

    private void HandleOnCurrentTurn()
    {
        context.UpdateVisitedHexesRecency();
        Think();
    }

    private async void Think()
    {
        int safetyLimit = 100;
        while (currentAP > 0 && safetyLimit-- > 0)
        {
            Debug.Log($"<color=yellow>{model.FactionType} started thinking...</color>");

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
                // Fix my chungus code someone

                var tcs = new TaskCompletionSource<bool>();
                Action onComplete = () => tcs.TrySetResult(true);
                controller.OnActionComplete += onComplete;

                Debug.Log($"<color=yellow>Best action: {bestAction} | Utility: {highestUtility}</color>");
                bestAction.Execute(context);

                await tcs.Task; //for actions/commands which have animation/duration/delay
                controller.OnActionComplete -= onComplete; 
                await Awaitable.WaitForSecondsAsync(0.5f); //so the ai doesnt look like its fking sicko doing actions back to back with no delay
            }
        }

        if (safetyLimit <= 0)
            Debug.LogError(context.Model.FactionType + " Think() hit safety limit possible infinite loop");

        // No ap left
        controller.TryEndTurn();
    }
}
