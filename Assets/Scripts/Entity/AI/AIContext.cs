using System.Collections.Generic;
using UnityEngine;

public class AIContext 
{
    public AIBrain Brain;
    public EntityModel Model;
    public EntityController Controller;

    public AIContext(AIBrain brain, EntityModel model, EntityController controller)
    {
        Brain = brain;
        Model = model;
        Controller = controller;
    }
}
