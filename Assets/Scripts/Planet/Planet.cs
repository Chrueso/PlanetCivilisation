using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Planet
{
    private Vector3 planetPos;
    private int resource;
    private int structures;

    public Vector3 Pos { get => planetPos; }
    public int Resource { get => resource; }
    public int Structures { get => structures; }
    

    public Planet(Vector3 pos, int resource, int structures)
    {
        this.planetPos = pos;
        this.resource = resource;
        this.structures = structures;
    }

    private void IncreaseResource()
    {
        resource++;
    }
    
    

    public void PlanetResourceGain() => IncreaseResource();
}
