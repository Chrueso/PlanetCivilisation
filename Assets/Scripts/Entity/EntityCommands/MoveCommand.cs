using UnityEngine;
using System;
using System.Threading.Tasks;
using DG.Tweening;

public class MoveCommand : ICommand
{
    private EntityController entityController;
    private EntityModel entityModel;
    private EntityView entityView;
    private GridHex originHex;
    private GridHex targetHex;
    private Action onComplete;

    public MoveCommand(EntityController entityController, EntityModel entityModel, EntityView entityView, GridHex targetHex, Action onComplete)
    {
        this.entityController = entityController;
        this.entityModel = entityModel;
        this.entityView = entityView;
        originHex = entityModel.CurrentHex;
        this.targetHex = targetHex;
        this.onComplete = onComplete;
    }

    public void Execute()
    {
        Move();  
    }

    private async void Move()
    {
        entityController.IsPerformingAction = true;

        entityModel.RemoveAP(1);
        entityModel.CurrentHex = targetHex;

        await entityView.Move(targetHex.WorldPosition, entityModel.yValue).AsyncWaitForCompletion();

        entityController.IsPerformingAction = false;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    public void Undo()
    {
        entityModel.AddAP(1);
        entityModel.CurrentHex = originHex;    
        entityView.Move(originHex.WorldPosition, entityModel.yValue);

        Debug.Log(this.ToString());
    }

    //For logging overrides ToString
    public override string ToString() => 
        $"{entityModel.FactionType} moved | " +
        $"Hex: {originHex.GridPosition} to {targetHex.GridPosition} | " +
        $"World: {originHex.WorldPosition} to {targetHex.WorldPosition}";
}
