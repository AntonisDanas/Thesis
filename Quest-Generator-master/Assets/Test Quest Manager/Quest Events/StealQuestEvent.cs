﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealQuestEvent : QuestEvent
{
    public StealQuestEvent(InteractableObject target)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        //(Target as InteractableCharacter).ActivateQuestMark();
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        //The stealing is handled by the EntityEventBroker
    }

    public override void SetInactive()
    {
        IsActive = false;
        //(Target as InteractableCharacter).DeactivateQuestMark();
    }

    public override bool CanProgressQuest()
    {
        return true;    // No condition to progress quest
    }
}
