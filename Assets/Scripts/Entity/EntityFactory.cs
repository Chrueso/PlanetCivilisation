using UnityEngine;

public class EntityFactory 
{
    private EntityView entityView;

    public void CreateEntity() //add entity preset as param then it makes the model
    {
        
    }

    public PlayerController CreatePlayer(PlanetData homePlanet, FactionType factionType) //replace with preset 
    {
        EntityModel model = new EntityModel(homePlanet, factionType);
        EntityView view = Object.Instantiate(entityView);
        PlayerController controller = new PlayerController(model, view);

        return controller;
    }
}
