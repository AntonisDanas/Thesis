using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : Interactable
{
    public string ObjectName { get { return m_objectName; } }


    [SerializeField] private string m_objectName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (EntityEventBroker.PickUpObject(this)) Destroy(gameObject);
    }
}
