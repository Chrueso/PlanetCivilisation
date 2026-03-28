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
        entityModel.RemoveAP(1);
        targetPlanet.SetFaction(entityModel.FactionType);
        entityModel.AddOwnedPlanets(targetPlanet);

        Debug.Log(this.ToString());
    }

    public void Undo()
    {
        entityModel.AddAP(1);
        targetPlanet.SetFaction(FactionType.Nothing);
        entityModel.RemoveOwnedPlanets(targetPlanet);

        Debug.Log($"{targetPlanet.PlanetName} is now uninhabited");
    }

    //For logging overrides ToString
    public override string ToString() =>
        $"{entityModel.FactionType} has colonized planet {targetPlanet.PlanetName}";
}
