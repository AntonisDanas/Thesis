using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : WorldEntity
{

    protected override void Start()
    {
        base.Start();
    }

    public virtual void Interact(WorldEntity invoker)
    {
        print("Interacting with base class.");
    }

}
