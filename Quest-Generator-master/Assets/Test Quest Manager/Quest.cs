using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    private List<QuestEvent> m_questEvents;
    private QuestEvent m_activeEvent;
    private QuestEvent m_nextEvent;
    private int m_questEventCounter;

    public Quest()
    {
        m_questEvents = new List<QuestEvent>();
        m_questEventCounter = 0;
    }

    public void AddQuestEvent(QuestEvent questEvent)
    {
        m_questEvents.Add(questEvent);
    }

    public void StartQuest()
    {
        if (!(m_questEvents.Count > 0)) return;

        m_questEvents[0].SetActive(this);
        m_activeEvent = m_questEvents[0];

        m_nextEvent = m_questEvents.Count >= 2 ? m_questEvents[1] : null;
        m_questEventCounter++;
    }

    public void QuestEventCompleted()
    {
        m_activeEvent.SetInactive();
        if (m_nextEvent == null)
        {
            CompleteQuest();
            return;
        }

        m_nextEvent.SetActive(this);
        m_activeEvent = m_nextEvent;
        m_questEventCounter++;

        Debug.Log(m_questEventCounter - 1 + " of " + m_questEvents.Count);

        m_nextEvent = m_questEventCounter < m_questEvents.Count ?
                                                m_questEvents[m_questEventCounter] :
                                                null;
    }

    public void CompleteQuest()
    {
        Debug.Log("Completed Quest! " + m_questEventCounter + " of " + m_questEvents.Count);
    }
}
