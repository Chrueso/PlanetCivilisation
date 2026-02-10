using System.Collections.Generic;
using UnityEngine;



public abstract class Faction
{
    public string factionName;
    public float money;

    public virtual void StartWorldTurn()
    {
        Debug.Log($"{factionName} is sucking dihh");
        EndWorldTurn();
    }

    public virtual void EndWorldTurn()
    {
        Debug.Log($"{factionName} has finished dihh sucking");
    }
}
