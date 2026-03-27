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
        entityView.Move(targetHex.WorldPosition, entityModel.yValue);

        Debug.Log(this.ToString());
    }

    public void Undo()
    {
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
