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
        entityModel.RemoveAP(1);

        Debug.Log(this.ToString());
    }

    public void Undo()
    {
        targetPlanet.SetFaction(FactionType.Nothing);
        entityModel.RemoveOwnedPlanets(targetPlanet);
        entityModel.AddAP(1);

        Debug.Log($"{targetPlanet.PlanetName} is now uninhabited");
    }

    //For logging overrides ToString
    public override string ToString() =>
        $"{entityModel.FactionType} has colonized planet {targetPlanet.PlanetName}";
}
