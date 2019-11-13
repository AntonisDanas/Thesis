using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEntity : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        EntityEventBroker.EnrollEntity(this);
    }
}
