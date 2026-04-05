using UnityEngine;
using System;

public class BuildStructureCommand : ICommand
{
    EntityController entityController;
    EntityModel entityModel;
    PlanetData targetPlanet;
    StructureType structure;
    private Action onComplete;
    
    public BuildStructureCommand(EntityController entityController, PlanetData planet, StructureType structure, Action onComplete)
    {
        this.entityController = entityController;
        this.entityModel = entityController.GetModel();
        this.targetPlanet = planet;
        this.structure = structure;
        this.onComplete = onComplete;
    }

    public void Execute()
    {
        entityController.IsPerformingAction = true;
        entityModel.RemoveAP(1);
        targetPlanet.BuildStructure(structure);
        entityController.IsPerformingAction = false;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    public override string ToString() =>
        $"{entityModel.FactionType} has built {structure} on planet {targetPlanet.PlanetName}";
}
