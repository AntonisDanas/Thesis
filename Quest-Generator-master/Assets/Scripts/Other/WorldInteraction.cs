using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace SideQuestGenerator
{
    public class WorldInteraction : MonoBehaviour
    {
        InteractableCharacter player;
        void Start()
        {
            EntityEventBroker.OnObjectPickUpFail += GetObjInterFail;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractableCharacter>();

        }

        void GetObjInter(InteractableObject obj)
        {
            print("Interacted with: " + obj.ObjectName);
        }

        void GetObjInterFail(InteractableObject obj)
        {
            print("Failed to interact with: " + obj.ObjectName);
        }


        void InteractWithCharacter(WorldEntity invoker, InteractableCharacter character)
        {
            print("Interacted with: " + character.CharacterName);
            EntityEventBroker.InteractWithCharacter(invoker, character);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                GetInteraction();
        }

        void GetInteraction()
        {
            Interactable interactable = GetInteractedObject();
            if (interactable != null && interactable is InteractableCharacter)
            {
                var i = interactable as InteractableCharacter;
                InteractWithCharacter(player, i);
                //i.TriggerQuestEvent(player);
            }
            else if (interactable != null && interactable is InteractableObject)
            {
                interactable.Interact(null);
            }
            else if (interactable != null && interactable is InteractableEnemy)
            {
                interactable.Interact(null);
            }
        }

        private Interactable GetInteractedObject()
        {
            Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8; // interactable layer is 8
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
                InteractableEnemy obj = hit.collider.gameObject.GetComponent<InteractableEnemy>();
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
}