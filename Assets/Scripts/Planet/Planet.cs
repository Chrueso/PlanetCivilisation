using UnityEngine;

public class Planet
{
    
    private int resource;
    private int structures;

    public int Resource { get => resource; }
    public int Structures { get => structures; }

    private void IncreaseResource()
    {
        resource++;
    }

    public void PlanetResourceGain() => IncreaseResource();
}
