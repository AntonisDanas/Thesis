using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherResourcesQuestEvent : QuestEvent
{
    private int m_gatherGoal = 0;
    private int m_currentGather = 0;

    public GatherResourcesQuestEvent(InteractableObject target)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        EntityEventBroker.OnObjectPickUpSuccess += ResourceGathered;
        // Fix how it appears to be on a quest. Maybe a shader
        //(Target as InteractableCharacter).ActivateQuestMark();

        m_gatherGoal = Random.Range(5, 10);
        m_currentGather = 0;
        Debug.Log("You need to gather " + m_gatherGoal + " of type " + (Target as InteractableObject).ObjectName);
    }

    public override void SetInactive()
    {
        IsActive = false;
        EntityEventBroker.OnObjectPickUpSuccess -= ResourceGathered;
        // Fix how it appears to be on a quest. Maybe a shader
        //(Target as InteractableCharacter).DeactivateQuestMark();
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        m_currentGather++;
        Debug.Log(m_currentGather + " out of " + m_gatherGoal + " " + (Target as InteractableObject).ObjectName + "s picked up");

        IsProgressing = false;
    }

    public override bool CanProgressQuest()
    {
        if (m_currentGather >= m_gatherGoal)
            return true;

        return false;
    }

    private void ResourceGathered(WorldEntity invoker, InteractableObject resource)
    {
        if (resource.ObjectName == (Target as InteractableObject).ObjectName)
        {
            IsProgressing = true;  // it is some sort of mutex
            TriggerEvent(invoker);
        }
    }
}
