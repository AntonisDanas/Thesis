using SideQuestGenerator.GraphEditor;
using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace SideQuestGenerator.InteractableHandling
{
    public class InteractableObject : Interactable
    {
        public string ObjectName { get { return m_objectName; } }

        [SerializeField] private SG_SpaceNode m_graphInstance;
        [SerializeField] private string m_objectName;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (m_graphInstance == null)
            {
                Debug.Log("No Graph Instance for Object");
                return;
            }

            m_objectName = m_graphInstance.NodeName;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Interact(WorldEntity invoker)
        {
            if (EntityEventBroker.PickUpObject(invoker, this)) //Destroy(gameObject);
                return;
        }

        public int GetIndexOfGraphInstance()
        {
            return m_graphInstance.Index;
        }
    }
}