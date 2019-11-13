using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestEvent
{
    public abstract void SetActive(Quest quest);
    public abstract void SetInactive();
    protected abstract void SubscribeToEventHandler();
    protected abstract void UnsubscribeToEventHandler();
    protected abstract void SetQuestMarker();
    protected abstract void DestroyQuestMarker();
}
