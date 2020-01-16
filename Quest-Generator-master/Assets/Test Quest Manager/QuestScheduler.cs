using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScheduler : MonoBehaviour
{
    private QuestManager m_questManager;
    private QuestGenerator m_questGenerator;

    //for debug purposes
    [SerializeField] private InteractableCharacter invoker;

    // Start is called before the first frame update
    void Start()
    {
        m_questManager = new QuestManager(this);
        m_questGenerator = new QuestGenerator(this);
        SubscribeToEntityEventBroker();

        PopulateTestQuest();
    }

    private void PopulateTestQuest()
    {
        //for debug purposes
        var testQuest = new Quest();
        var interactableCharacters = FindObjectsOfType<InteractableCharacter>();
        InteractableCharacter sk = null;
        InteractableCharacter p = null;
        InteractableCharacter t = null;
        foreach (var item in interactableCharacters)
        {
            if (item.CharacterName.Equals("Shopkeeper"))
                sk = item;
            if (item.CharacterName.Equals("Player"))
                p = item;
            if (item.CharacterName.Equals("Smith Jones"))
                t = item;
        }
        var iq = new InvokeQuestEvent(sk);
        var aq = new AssassinationEvent(t);
        var cq = new CompleteQuestEvent(sk);

        testQuest.AddQuestEvent(iq);
        testQuest.AddQuestEvent(aq);
        testQuest.AddQuestEvent(cq);

        m_questManager.AddPendingQuest(testQuest);

    }

    private void SubscribeToEntityEventBroker()
    {
        EntityEventBroker.OnEntityDeath += WorldEntityDied;
        EntityEventBroker.OnObjectPickUpSuccess += InteractableObjectPicked;
        EntityEventBroker.OnQuestInvoked += InvokeQuest;
        EntityEventBroker.OnQuestCompleted += CompleteQuest;
    }

    public void WorldEntityDied(WorldEntity invoker, WorldEntity target)
    {
        //Check if target was part of a Quest
        //If yes then QuestManager has to deal with it (async)
        var qe = m_questManager.IsWorldEntityPartOfQuest(target);
        if (qe != null)
        //if (true)
        {
            StartCoroutine(m_questManager.ProgressQuest(invoker, qe));
        }

        //Check if QuestGenerator can spawn another quest
    }

    public void InteractableObjectPicked(WorldEntity invoker, InteractableObject interactableObject)
    {

    }

    public void InvokeQuest(Quest quest)
    {
        Debug.Log("Quest Invoked");
        m_questManager.ActivateQuest(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        Debug.Log("Quest Completed");
        m_questManager.CompleteQuest(quest);
    }
}
