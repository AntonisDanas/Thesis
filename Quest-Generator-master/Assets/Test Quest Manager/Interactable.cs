using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : WorldEntity
{

    void Update()
    {

    }

    public virtual void Interact()
    {
        print("Interacting with base class.");
    }

}
