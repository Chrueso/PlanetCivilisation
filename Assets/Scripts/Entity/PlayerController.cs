using UnityEngine;

public class PlayerController : IEntityController
{
    EntityModel model;
    EntityView view;
    
    public PlayerController(EntityModel model, EntityView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Move(GridHex targetHex)
    {
        ICommand command = new MoveCommand(model, view, targetHex);
        CommandInvoker.ExecuteCommand(command);

        view.Move(targetHex.WorldPosition);
    }

    public void Colonize(PlanetData planet)
    {
        if (planet.FactionType == FactionType.Nothing)
        {
            ICommand command = new ColonizeCommand(model, planet);
            CommandInvoker.ExecuteCommand(command);
        }
        else
        {
            Debug.Log("You cannot colonize a owned planet!");
        }
    }

    public void Attack(PlanetData planet)
    {
      
    }

    public void BuildStructure(PlanetData planet, StructureType structure)
    {

    }

    // Diplomacy
    public void Trade(PlanetData planet)
    {

    }

    public void Gift(PlanetData planet)
    {

    }

    public void Agreement(PlanetData planet)
    {

    }
}
