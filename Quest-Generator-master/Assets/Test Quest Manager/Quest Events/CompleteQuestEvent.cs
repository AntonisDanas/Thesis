using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteQuestEvent : QuestEvent
{
    public CompleteQuestEvent(WorldEntity target)
    {
        Target = target;
        IsActive = false;
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        if (!IsActive) return;
        if (Quest == null) return;

        EntityEventBroker.QuestCompleted(Quest);
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        (Target as InteractableCharacter).ActivateQuestMark();
    }

    public override void SetInactive()
    {
        IsActive = false;
        (Target as InteractableCharacter).DeactivateQuestMark();
    }
    
}
