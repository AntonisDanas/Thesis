using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;


    private List<WorldEntity> m_worldEntities;
    private Quest quest;

    private void Awake()
    {
        m_worldEntities = new List<WorldEntity>();
        EntityEventBroker.OnEntityEnroll += EnrollEntity;
        EntityEventBroker.OnEntityDeath += EntityDied;

        //test
        quest = null;
    }

    private void Update()
    {
        if (quest == null && m_worldEntities.Count > 0)
        {

            // test
            quest = new Quest();
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<WorldEntity>();
            var cube1 = Cube1.GetComponent<QuestEntity>();
            var invoke1 = new InvokeQuestEvent(player, cube1);
            quest.AddQuestEvent(invoke1);

            var cube2 = Cube2.GetComponent<QuestEntity>();
            var assassination = new AssassinationEvent(player, cube2);
            quest.AddQuestEvent(assassination);

            var cube3 = Cube3.GetComponent<QuestEntity>();
            var invoke2 = new InvokeQuestEvent(player, cube3);
            quest.AddQuestEvent(invoke2);

            quest.StartQuest();
        }
    }

    public void SetQuestToEntity(QuestEntity entity)
    {
        //entity.HasQuestEvent();
    }

    private void EnrollEntity(WorldEntity entity)
    {
        m_worldEntities.Add(entity);
        Debug.Log(entity.name + " added to the QuestGenerator");
    }

    private void EntityDied(WorldEntity invoker, WorldEntity recepient)
    {
        //print(invoker.name + " killed " + recepient.name);
    }
}
