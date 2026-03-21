using UnityEngine;

public class ColonizeCommand : ICommand
{
    private EntityData entityModel;
    private PlanetData targetPlanet; 

    public ColonizeCommand(EntityData entityModel, PlanetData planet)
    {
        this.entityModel = entityModel;
        this.targetPlanet = planet;
    }

    public void Execute()
    {
        targetPlanet.SetFaction(entityModel.FactionType);
        entityModel.AddOwnedPlanets(targetPlanet);

        Debug.Log(this.ToString());
    }

    public void Undo()
    {
        targetPlanet.SetFaction(FactionType.Nothing);
        entityModel.RemoveOwnedPlanets(targetPlanet);

        Debug.Log($"{targetPlanet.PlanetName} is now uninhabited");
    }

    //For logging overrides ToString
    public override string ToString() =>
        $"[{entityModel.FactionType}] Has colonized planet {targetPlanet.PlanetName}";
}
