//public enum Shiptype
//{
//    Scout,
//    Assault,
//    Worker
//}

//new shipType class 
public class ShipType
{
    public string name { get; private set; }
    public int attackPower { get; private set; }
    public bool canAttack { get; private set; }

    public ShipType(string name, int attackPower, bool canAttack)
    {
        this.name = name;
        this.attackPower = attackPower;
        this.canAttack = canAttack;
    }
}

public static class ShipTypes
{
    public static ShipType Scout = new ShipType("Scout", 1, false);
    public static ShipType Assault = new ShipType("Assault", 3, true);
    public static ShipType Worker = new ShipType("Worker", 1, false);

    public static ShipType[] AllTypes = new ShipType[] { Scout, Assault, Worker };
}