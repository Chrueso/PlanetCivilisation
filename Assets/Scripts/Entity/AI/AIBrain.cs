using System.Collections.Generic;
using UnityEngine;

public class AIBrain
{
    private List<AIAction> actions = new List<AIAction>();
    private AIContext context;

    private EntityController controller;
    private EntityModel model;

    private int currentAP => model.CurrentAP;
    private bool isCurrentTurn => controller.IsCurrentTurn;

    private GridHex currentHex => model.CurrentHex;

    private EventBinding<TurnChangeEvent> turnChangeEventBinding;


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

        turnChangeEventBinding = new EventBinding<TurnChangeEvent>(HandleTurnChange);
        EventBus<TurnChangeEvent>.Register(turnChangeEventBinding);
    }

    private void HandleTurnChange(TurnChangeEvent turnChangeEvent)
    {
        if (isCurrentTurn) Think();
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
                bestAction.Execute(context);

                Debug.Log(context.Model.FactionType + " Best Action: " + bestAction.ToString() + " | Utility: " + highestUtility.ToString());
            }
        }

        if (safetyLimit <= 0)
            Debug.LogError(context.Model.FactionType + " Think() hit safety limit — possible infinite loop.");

        // No ap left
        controller.TryEndTurn();
    }

    //public void Think()
    //{
    //    AIAction bestAction = null;

    //    var planet = currentHex.Occupant as PlanetData;

    //    bool isOnPlanet = planet != null;
    //    bool isUninhabited = isOnPlanet && planet.FactionType == FactionType.Nothing;
    //    bool isOwnedByMe = isOnPlanet && planet.FactionType == controller.GetFaction();
    //    bool isOwnedByEnemy = isOnPlanet && !isUninhabited && !isOwnedByMe;

    //    if (!isOnPlanet) { bestAction = moveAction; }
    //    else if (isUninhabited) { bestAction = colonizeAction; }
    //    else if (isOwnedByMe) { bestAction = CalculateUtility(myPlanetActions); }
    //    else if (isOwnedByEnemy) { bestAction = CalculateUtility(enemyPlanetActions); }

    //    if (bestAction != null)
    //    {
    //        bestAction.Execute(context);
    //    }
    //}

    //public AIAction CalculateUtility(List<AIAction> actions)
    //{
    //    AIAction bestAction = null;
    //    float highestUtility = float.MinValue;

    //    foreach (var action in actions)
    //    {
    //        float utility = action.CalculateUtility(context);

    //        if (utility > highestUtility)
    //        {
    //            highestUtility = utility;
    //            bestAction = action;
    //        }
    //    }

    //    return bestAction;
    //}
}
