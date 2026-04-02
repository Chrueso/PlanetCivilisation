using UnityEngine;
using System;

public class AttackCommand : ICommand
{
    private BattleManager battleManager;
    private EntityController entityController;
    private EntityModel entityModel;
    private PlanetData targetPlanet;
    private Action onComplete;

    public AttackCommand(BattleManager battleManager, EntityController entityController, EntityModel entityModel, PlanetData planet, Action onComplete)
    {
        this.battleManager = battleManager;
        this.entityController = entityController;
        this.entityModel = entityModel;
        this.targetPlanet = planet;
        this.onComplete = onComplete;
    }

    public void Execute()
    {
        entityController.IsPerformingAction = true;

        entityModel.RemoveAP(1);

        BattleResult result = battleManager.Battle(entityModel.Ships, targetPlanet);
        if (result.AttackerWon)
        {
            Debug.Log($"{entityModel.FactionType} has taken over planet {targetPlanet.FactionType}'s planet {targetPlanet.PlanetName}");
            targetPlanet.SetFaction(entityModel.FactionType);
            entityModel.AddOwnedPlanets(targetPlanet);
            targetPlanet.View.ShowTakenOverEffect();
        }
        else
        {
            Debug.Log($"{entityModel.FactionType} failed to take over planet {targetPlanet.PlanetName}");
        }

        entityController.IsPerformingAction = false;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    //For logging overrides ToString
    public override string ToString() =>
        $"{entityModel.FactionType} started attack on {targetPlanet.FactionType}'s planet {targetPlanet.PlanetName}";
}