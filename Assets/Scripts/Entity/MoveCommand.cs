using UnityEngine;

public class MoveCommand : ICommand
{
    private EntityModel entityModel;
    private EntityView entityView;
    private GridHex originHex;
    private GridHex targetHex;
    
    public MoveCommand(EntityModel entityModel, EntityView entityView, GridHex targetHex)
    {
        this.entityModel = entityModel;
        this.entityView = entityView;
        originHex = entityModel.CurrentHex;
        this.targetHex = targetHex;
    }

    public void Execute()
    {
        entityModel.CurrentHex = targetHex;
        entityView.Move(targetHex.WorldPosition);
    }

    public void Undo()
    {
        entityModel.CurrentHex = originHex;    
        entityView.Move(originHex.WorldPosition);
    }

    //For logging overrides ToString
    public override string ToString() => 
        $"[{entityModel.FactionType}] Command Ship | " +
        $"Hex: {originHex.GridPosition} to {targetHex.GridPosition} | " +
        $"World: {originHex.WorldPosition} to {targetHex.WorldPosition}";
}
