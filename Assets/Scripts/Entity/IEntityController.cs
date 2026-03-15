using UnityEngine;

public interface IEntityController 
{
    public void Move(GridHex hex);

    public void BuildStructure(PlanetData planet);

    public void BuildShip(PlanetData planet);

    public void Colonize(PlanetData planet);

    public void Attack(PlanetData planet);

    // Diplomacy
    public void Trade(PlanetData planet);

    public void Gift(PlanetData planet);

    public void Agreement(PlanetData planet);

}
