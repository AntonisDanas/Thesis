using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesQuestEvent : QuestEvent
{
    private int m_killGoal;
    private int m_currentKills = 0;

    public KillEnemiesQuestEvent(InteractableEnemy target, int goal)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
        m_killGoal = goal;
    }

    public KillEnemiesQuestEvent(InteractableEnemy target, int goal, QuestEventDescription description)
    {
        Target = target;
        IsActive = false;
        IsProgressing = false;
        Description = description;
        m_killGoal = goal;
    }

    public override void SetActive(Quest quest)
    {
        IsActive = true;
        Quest = quest;
        EntityEventBroker.OnEnemyKilled += EnemyKilled;
        // Fix how it appears to be on a quest. Maybe a shader
        //(Target as InteractableCharacter).ActivateQuestMark();

        m_currentKills = 0;
        Debug.Log("You need to kill " + m_killGoal + " of type " + (Target as InteractableEnemy).EnemyName);
    }

    public override void SetInactive()
    {
        IsActive = false;
        EntityEventBroker.OnEnemyKilled -= EnemyKilled;
        // Fix how it appears to be on a quest. Maybe a shader
        //(Target as InteractableCharacter).DeactivateQuestMark();
    }

    public override void TriggerEvent(WorldEntity invoker)
    {
        m_currentKills++;
        Debug.Log(m_currentKills + " out of " + m_killGoal + " " + (Target as InteractableEnemy).EnemyName + "s killed");

        IsProgressing = false;
    }

    public override bool CanProgressQuest()
    {
        if (m_currentKills >= m_killGoal)
            return true;

        return false;
    }

    private void EnemyKilled(WorldEntity invoker, InteractableEnemy enemy)
    {
        if (enemy.EnemyName == (Target as InteractableEnemy).EnemyName)
        {
            IsProgressing = true;  // it is some sort of mutex
            TriggerEvent(invoker);
        }
    }

    public override QuestEventDescription GetQuestEventDescription()
    {
        var des = new QuestEventDescription();
        des.DescriptionLabel = "You need to kill " + m_killGoal + " of " + (Target as InteractableEnemy).EnemyName;
        des.ButtonLabel = ""; // no button needed

        return des;
    }
}
