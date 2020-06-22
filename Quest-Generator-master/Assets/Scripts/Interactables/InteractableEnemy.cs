using SideQuestGenerator.GraphEditor;
using SideQuestGenerator.Scheduling;
using UnityEngine;

namespace SideQuestGenerator.InteractableHandling
{
    public class InteractableEnemy : Interactable
    {
        public string EnemyName { get { return m_enemyName; } }

        [SerializeField] private SG_EnemyNode m_graphInstance;
        [SerializeField] private string m_enemyName;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (m_graphInstance == null)
            {
                Debug.Log("No Graph Instance for Enemy");
                return;
            }

            m_enemyName = m_graphInstance.nodeName;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Interact(WorldEntity invoker)
        {
            EntityEventBroker.EnemyKilled(invoker, this); //Destroy(gameObject);
        }

        public int GetIndexOfGraphInstance()
        {
            return m_graphInstance.Index;
        }
    }
}