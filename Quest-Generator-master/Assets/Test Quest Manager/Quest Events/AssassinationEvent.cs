using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationEvent : QuestEvent
{

    public AssassinationEvent(WorldEntity target)
    {
        Target = target;
        IsActive = false;
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

    private void CheckIfEventTriggered(WorldEntity invoker, WorldEntity recepient)
    {
        if (!IsActive) return;

        if ((recepient as InteractableCharacter) != Target) return;
        if (!(invoker is InteractableCharacter)) return;

        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractableCharacter>();
        if ((invoker as InteractableCharacter) == player)
        {
            Debug.Log("Player killed target");
            Quest.QuestEventCompleted();
        }
        else
            Debug.Log("Quest Failed");
    }
}
