using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeQuestEvent : QuestEvent
{

    public InvokeQuestEvent(InteractableCharacter target)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
        Description = null;
    }

    public InvokeQuestEvent(InteractableCharacter target, QuestEventDescription description)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
        Description = description;
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        if (!IsActive) return;
        if (Quest == null) return;

        EntityEventBroker.InvokeQuest(Quest);
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

    public override bool CanProgressQuest()
    {
        return true;    // No condition to progress quest
    }

    public override QuestEventDescription GetQuestEventDescription()
    {
        return Description;
    }
}
