using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferObjectQuestEvent : QuestEvent
{
    private InteractableObject m_objectToTransfer;

    public TransferObjectQuestEvent(InteractableCharacter target, InteractableObject obj)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;

        m_objectToTransfer = obj;
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        (Target as InteractableCharacter).ActivateQuestMark();
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        //TODO check if player has object
        EntityEventBroker.TransferObject(Target as InteractableCharacter, m_objectToTransfer);
        Quest.QuestEventCompleted();
    }

    public override void SetInactive()
    {
        IsActive = false;
        (Target as InteractableCharacter).DeactivateQuestMark();
    }

    public override bool CanProgressQuest()
    {
        return true;    
    }
}
