using UnityEngine;

public interface IEntityController
{
    public void Move(GridHex hex);

    public void Colonize(PlanetData planet);

    public void Attack(PlanetData planet);

    public void BuildStructure(PlanetData planet, StructureType structure);

    
    // Diplomacy
    public void Trade(PlanetData planet);

    public void Gift(PlanetData planet);

    public void Agreement(PlanetData planet);
}
