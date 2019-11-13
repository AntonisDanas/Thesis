using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeQuestEvent : QuestEvent
{
    private WorldEntity m_invoker = null;
    private WorldEntity m_target = null;
    private bool m_isActive = false;
    private Quest m_questReference = null;

    public InvokeQuestEvent(WorldEntity invoker, WorldEntity target)
    {
        m_invoker = invoker;
        m_target = target;
        m_isActive = false;
    }

    public override void SetActive(Quest quest)
    {
        m_isActive = true;
        SubscribeToEventHandler();
        SetQuestMarker();
        m_questReference = quest;
    }

    public override void SetInactive()
    {
        m_isActive = false;
        UnsubscribeToEventHandler();
        DestroyQuestMarker();
    }

    protected override void DestroyQuestMarker()
    {
        (m_target as QuestEntity).DestroyQuestMarker();
    }

    protected override void SetQuestMarker()
    {
        (m_target as QuestEntity).SetQuestMarker();
    }

    protected override void SubscribeToEventHandler()
    {
        // for now subscribe to death event
        EntityEventBroker.OnEntityDeath += CheckIfEventTriggered;
    }

    protected override void UnsubscribeToEventHandler()
    {
        // for now subscribe to death event
        EntityEventBroker.OnEntityDeath -= CheckIfEventTriggered;
    }

    private void CheckIfEventTriggered(WorldEntity invoker, WorldEntity recepient)
    {
        if (!m_isActive) return;

        if (invoker == m_invoker && recepient == m_target)
        {
            Debug.Log("Quest: " + invoker.name + " invoked quest from " + recepient.name);
            m_questReference.QuestEventCompleted();
        }
    }
}
