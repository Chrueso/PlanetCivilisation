using UnityEngine;
using System;

public class ColonizeCommand : ICommand
{
    private EntityController entityController;
    private EntityModel entityModel;
    private PlanetData targetPlanet;
    private Action onComplete;

    public ColonizeCommand(EntityController entityController, EntityModel entityModel, PlanetData planet, Action onComplete)
    {
        this.entityController = entityController;
        this.entityModel = entityModel;
        this.targetPlanet = planet;
        this.onComplete = onComplete;
    }

    public void Execute()
    {
        entityController.IsPerformingAction = true;

        entityModel.RemoveAP(1);
        targetPlanet.SetFaction(entityModel.FactionType);
        entityModel.AddOwnedPlanets(targetPlanet);
        targetPlanet.View.ShowColonizeEffect();
        entityController.IsPerformingAction = false;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    //public void Undo()
    //{
    //    entityModel.AddAP(1);
    //    targetPlanet.SetFaction(FactionType.Nothing);
    //    entityModel.RemoveOwnedPlanets(targetPlanet);

    //    Debug.Log($"{targetPlanet.PlanetName} is now uninhabited");
    //}

    //For logging overrides ToString
    public override string ToString() =>
        $"{entityModel.FactionType} has colonized planet {targetPlanet.PlanetName}";
}
