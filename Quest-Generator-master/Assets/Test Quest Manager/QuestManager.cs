using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    //public static bool CanAddQuest { get; private set; }

    private QuestScheduler m_questScheduler;

    private List<Quest> m_pendingQuests;
    private List<Quest> m_activeQuests;
    private List<QuestEvent> m_questEvents;


    public QuestManager(QuestScheduler qs)
    {
        m_questScheduler = qs;
        //CanAddQuest = true;
        m_pendingQuests = new List<Quest>();
        m_activeQuests = new List<Quest>();
        m_questEvents = new List<QuestEvent>();
    }

    public void AddPendingQuest(Quest pq)
    {
        if (pq.InitQuest()) m_pendingQuests.Add(pq);

        foreach (var item in pq.QuestEvents)
        {
            m_questEvents.Add(item);
        }
    }

    public void ActivateQuest(Quest aq)
    {
        aq.StartQuest();
        m_pendingQuests.Remove(aq);
        m_activeQuests.Add(aq);
    }

    public void CompleteQuest(Quest cq)
    {
        cq.EndQuest();

        m_activeQuests.Remove(cq);
        foreach (var item in cq.QuestEvents)
        {
            m_questEvents.Remove(item);
        }
    }

    public QuestEvent IsWorldEntityPartOfQuest(WorldEntity entity)
    {
        foreach (var questEvent in m_questEvents)
        {
            if (!questEvent.IsActive) continue;

            if (questEvent.Target == entity)
            {
                return questEvent;
            }
        }

        return null;
    }

    public IEnumerator ProgressQuest(WorldEntity invoker, QuestEvent questEvent)
    {
        var quest = questEvent.Quest;
        quest.QuestEventCompleted();

        yield return null;
    }
}
