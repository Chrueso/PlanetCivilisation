using DG.Tweening;
using System;
using UnityEngine;

public class MoveScoutShipCommand : ICommand
{
    private EntityController entityController;
    private EntityModel entityModel;
    private EntityScoutShipView entityView;
    private GridHex originHex;
    private GridHex targetHex;
    private Action onComplete;
    public MoveScoutShipCommand(EntityController entityController, EntityModel entityModel, EntityScoutShipView entityView, GridHex targetHex, Action onComplete)
    {
        this.entityController = entityController;
        this.entityModel = entityModel;
        this.entityView = entityView;
        this.originHex = entityModel.CurrentHex;
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
        await entityView.Move(targetHex.WorldPosition, entityModel.yValue).AsyncWaitForCompletion();
        entityModel.CurrentHex = targetHex;
        await entityView.MoveBack(targetHex.WorldPosition, entityModel.yValue).AsyncWaitForCompletion();

        entityController.IsPerformingAction = false;
        entityView.Release();
        entityModel.CurrentHex = originHex;
        onComplete?.Invoke();
        Debug.Log(this.ToString());
    }

    public override string ToString() =>
        $"{entityModel.FactionType} moved | " +
        $"Hex: {originHex.GridPosition} to {targetHex.GridPosition} | " +
        $"World: {originHex.WorldPosition} to {targetHex.WorldPosition}";
}
