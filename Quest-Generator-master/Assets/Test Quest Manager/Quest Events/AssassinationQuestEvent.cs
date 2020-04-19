using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationQuestEvent : QuestEvent
{

    public AssassinationQuestEvent(WorldEntity target)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        (Target as InteractableCharacter).ActivateQuestMark();
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        //TODO
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
}
