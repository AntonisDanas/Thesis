using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestEvent
{

    public Quest Quest { get; protected set; }
    public WorldEntity Target { get; protected set; }
    public bool IsActive { get; protected set; }
    public abstract void TriggerEvent(WorldEntity invoker);
    public abstract void SetActive(Quest quest);
    public abstract void SetInactive();
}
