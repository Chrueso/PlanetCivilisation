using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Colonize")]
public class AIColonizeAction : AIAction
{
    EntityModel model;
    EntityController controller;
    PlanetData planet;  

    public override void Init(AIContext context)
    {
        model = context.Model;
        controller = context.Controller;
    }

    public override float CalculateUtility(AIContext context)
    {
        GridHex currentHex = model.CurrentHex;
        planet = currentHex.Occupant as PlanetData;
        bool isOnPlanet = planet != null;
        bool isUninhabited = isOnPlanet && planet.FactionType == FactionType.Nothing;

        if (isUninhabited) return 1;

        return -1;
    }

    public override void Execute(AIContext context)
    {
        controller.TryColonize(planet);
    }
}
