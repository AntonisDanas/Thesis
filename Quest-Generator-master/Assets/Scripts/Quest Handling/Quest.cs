﻿using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.Scheduling;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator.QuestHandling
{
    public class Quest
    {
        public List<QuestEvent> QuestEvents { get; private set; }
        private QuestEvent m_activeEvent;
        private QuestEvent m_nextEvent;
        private int m_questEventCounter;
        private bool m_isInit;

        public Quest()
        {
            QuestEvents = new List<QuestEvent>();
            m_questEventCounter = 0;
            m_isInit = false;
        }

        public void AddQuestEvent(QuestEvent questEvent)
        {
            QuestEvents.Add(questEvent);
        }

        public bool InitQuest()
        {
            if (!(QuestEvents.Count > 0)) return false;

            QuestEvents[0].SetActive(this);
            m_activeEvent = QuestEvents[0];

            m_nextEvent = QuestEvents.Count >= 2 ? QuestEvents[1] : null;
            m_questEventCounter++;

            if (!(QuestEvents[0] is InvokeQuestEvent)) return false;

            InteractableCharacter target = QuestEvents[0].Target as InteractableCharacter;

            target.AddQuestEvent(QuestEvents[0] as InvokeQuestEvent);

            m_isInit = true;

            return true;
        }

        public void StartQuest()
        {
            if (!m_isInit) Debug.LogWarning("Quest has not been initialized. Try InitQuest()");

            if (!(QuestEvents.Count > 1)) return;

            m_activeEvent.SetInactive();
            QuestEvents[1].SetActive(this);
            m_activeEvent = QuestEvents[1];

            m_nextEvent = QuestEvents.Count >= 3 ? QuestEvents[2] : null;
            m_questEventCounter++;

            InteractableCharacter target = QuestEvents[QuestEvents.Count - 1].Target as InteractableCharacter;

            target.AddQuestEvent(QuestEvents[QuestEvents.Count - 1] as CompleteQuestEvent);
        }

        public void QuestEventCompleted()
        {
            m_activeEvent.SetInactive();

            m_nextEvent.SetActive(this);
            if (m_nextEvent.Target is InteractableCharacter)
            {
                InteractableCharacter ic = m_nextEvent.Target as InteractableCharacter;
                ic.AddQuestEvent(m_nextEvent);
            }


            m_activeEvent = m_nextEvent;
            m_questEventCounter++;

            m_nextEvent = m_questEventCounter < QuestEvents.Count ?
                                                    QuestEvents[m_questEventCounter] :
                                                    null;
        }

        public void CompleteQuest()
        {
            EntityEventBroker.QuestCompleted(this);
        }

        public void EndQuest()
        {
            foreach (var item in QuestEvents)
            {
                item.SetInactive();
            }
        }

    }
}