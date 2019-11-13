﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteraction : MonoBehaviour
{

    void Start()
    {
        EntityEventBroker.OnObjectPickUpSuccess += GetObjInter;
        EntityEventBroker.OnObjectPickUpFail += GetObjInterFail;

        EntityEventBroker.OnCharacterInteract += GetCharInter;
    }

    void GetObjInter(InteractableObject obj)
    {
        print("Interacted with: " + obj.ObjectName);
    }

    void GetObjInterFail(InteractableObject obj)
    {
        print("Failed to interact with: " + obj.ObjectName);
    }


    void GetCharInter(InteractableCharacter obj)
    {
        print("Interacted with: " + obj.CharacterName);
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetMouseButtonDown(0))// && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            GetInteraction();
	}

    void GetInteraction()
    {
        Interactable interactable = GetInteractedObject();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private Interactable GetInteractedObject()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << (int)8; // interactable layer is 8
        RaycastHit[] hits;
        hits = Physics.RaycastAll(interactionRay, 1000f, layerMask);

        foreach (var hit in hits)
        {
            InteractableObject obj = hit.collider.gameObject.GetComponent<InteractableObject>();
            if (obj != null)
            {
                return obj as Interactable;
            }
        }

        foreach (var hit in hits)
        {
            InteractableCharacter obj = hit.collider.gameObject.GetComponent<InteractableCharacter>();
            if (obj != null)
            {
                return obj as Interactable;
            }
        }

        return null;
    }
}
