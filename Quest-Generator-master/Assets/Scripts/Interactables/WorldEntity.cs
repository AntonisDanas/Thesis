using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace SideQuestGenerator.InteractableHandling
{
    public class WorldEntity : MonoBehaviour
    {
        // Start is called before the first frame update
        protected virtual void Start()
        {
            EntityEventBroker.EnrollEntity(this);
        }
    }
}