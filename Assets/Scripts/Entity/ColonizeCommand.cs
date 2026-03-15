using UnityEngine;

public class ColonizeCommand : ICommand
{
    private EntityModel entityModel;
    private PlanetData targetPlanet; 

    public ColonizeCommand(EntityModel entityModel, PlanetData planet)
    {
        this.entityModel = entityModel;
        this.targetPlanet = planet;
    }

    public void Execute()
    {
        targetPlanet.SetFaction(entityModel.FactionType);
        entityModel.AddOwnedPlanets(targetPlanet);
    }

    public void Undo()
    {
        targetPlanet.SetFaction(FactionType.Nothing);
        entityModel.RemoveOwnedPlanets(targetPlanet);
    }
}
