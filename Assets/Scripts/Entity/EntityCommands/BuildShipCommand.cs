using System;
using UnityEngine;

public class BuildShipCommand : ICommand
{
    private EntityController entityController;
    private EntityModel entityModel;
    private Action onComplete;
    public BuildShipCommand(EntityController entityController, EntityModel entityModel, Action onComplete)
    {
        this.entityController = entityController;
        this.entityModel = entityModel;
        this.onComplete = onComplete;
    }
    public void Execute()
    {
        entityController.IsPerformingAction = true;

        entityModel.RemoveAP(1);
        entityController.IsPerformingAction = false;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    public override string ToString() =>
        $"{entityModel.FactionType} built ship";
        
}
