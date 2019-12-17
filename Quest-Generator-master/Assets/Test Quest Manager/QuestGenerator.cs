using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    private List<WorldEntity> m_worldEntities;

    private void Awake()
    {
        m_worldEntities = new List<WorldEntity>();
        EntityEventBroker.OnEntityEnroll += EnrollEntity;
        EntityEventBroker.OnEntityDeath += EntityDied;
    }

    private void Update()
    {

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
