using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScheduler : MonoBehaviour
{
    public float MinQuestGenerationThreshold;
    public float MaxQuestGenerationThreshold;
    public int QuestPoolSize;
    public float QuestPoolGenerationMultiplier;
    public float RuleCostMultiplier;
    public float RuleCountMultiplier;

    private QuestManager m_questManager;
    private QuestGenerator m_questGenerator;
    private Graph m_graph;

    private float m_timePassed;
    private float m_timeGoal;


    void Awake()
    {
        m_questManager = new QuestManager(this);
        m_questGenerator = new QuestGenerator(this, 
                                    FindObjectOfType<GraphHandler>(), 
                                    QuestPoolSize,
                                    QuestPoolGenerationMultiplier,
                                    RuleCostMultiplier,
                                    RuleCountMultiplier);

        SubscribeToEntityEventBroker();

        m_timePassed = 0f;
        m_timeGoal = UnityEngine.Random.Range(MinQuestGenerationThreshold, MaxQuestGenerationThreshold);

        //for debug purposes
        //PopulateTestQuest();

        //for debug purposes
        //PopulateTestWorldGraph();

        //for debug purposes
        //RunCustomReaction();

        //for debug purposes
        //PopulateResourceGatherQuest();

        //for debug purposes
        //PopulateKillEnemiesQuest();
    }

    private void Update()
    {
        // for debug
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(m_questGenerator.GenerateNewQuest());
        //    //PopulateKillEnemiesQuest();
        //}

        m_timePassed += Time.deltaTime;

        if (m_timePassed > m_timeGoal)
        {
            Debug.Log("Trying to generate Quest");
            StartCoroutine(m_questGenerator.GenerateNewQuest());

            m_timePassed = 0f;
            m_timeGoal = UnityEngine.Random.Range(MinQuestGenerationThreshold, MaxQuestGenerationThreshold);
        }
    }

    private void PopulateTestWorldGraph()
    {
        m_graph = new Graph();
        int index = 1;
        string label = "NPC";
        Dictionary<String, object> attributes1 = new Dictionary<string, object>();
        attributes1.Add("Name", "Regal Regis");
        attributes1.Add("Alive", true);
        attributes1.Add("Rank", "King");

        Vertex regal_regis = new Vertex(index, label, attributes1);
        index++;

        Dictionary<String, object> attributes2 = new Dictionary<string, object>();
        attributes2.Add("Name", "Fancy Fanny");
        attributes2.Add("Alive", true);
        attributes2.Add("Rank", "Queen");

        Vertex fancy_fanny = new Vertex(index, label, attributes2);
        index++;

        Dictionary<String, object> attributes3 = new Dictionary<string, object>();
        attributes3.Add("Name", "Smug Smith");
        attributes3.Add("Alive", true);
        attributes3.Add("Rank", "Lord");

        Vertex smug_smith = new Vertex(index, label, attributes3);
        index++;

        m_graph.AddVertex(regal_regis);
        m_graph.AddVertex(fancy_fanny);
        m_graph.AddVertex(smug_smith);

        m_graph.SetRelation(regal_regis, fancy_fanny, "Hates", "Affair");
        m_graph.SetRelation(fancy_fanny, regal_regis, "Hates", "Ugly");

        m_graph.SetRelation(fancy_fanny, smug_smith, "Loves", "Looks");
        m_graph.SetRelation(smug_smith, fancy_fanny, "Loves", "Money");

        m_graph.SetRelation(regal_regis, smug_smith, "Dislikes", "Affair");
        m_graph.SetRelation(smug_smith, regal_regis, "Dislikes", "Arrogant");
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
        var aq = new AssassinationQuestEvent(t);
        var cq = new CompleteQuestEvent(sk);

        testQuest.AddQuestEvent(iq);
        testQuest.AddQuestEvent(aq);
        testQuest.AddQuestEvent(cq);

        m_questManager.AddPendingQuest(testQuest);

    }

    private void RunCustomReaction()
    {
        CustomReactionQuestEvent cr = new CustomReactionQuestEvent(null);
        cr.TriggerEvent(null);
    }

    private void PopulateResourceGatherQuest()
    {
        //for debug purposes
        var testQuest = new Quest();
        var interactableObjects = FindObjectsOfType<InteractableObject>();
        InteractableObject obj= null;

        foreach (var item in interactableObjects)
        {
            if (item.ObjectName.Equals("Flower"))
            {
                obj = item;
                break;
            }
        }

        var interactableCharacters = FindObjectsOfType<InteractableCharacter>();
        InteractableCharacter owner = null;

        foreach (var item in interactableCharacters)
        {
            if (item.CharacterName.Equals("Owner"))
            {
                owner = item;
                break;
            }
        }

        var iq = new InvokeQuestEvent(owner);
        var gr = new GatherResourcesQuestEvent(obj);
        var cq = new CompleteQuestEvent(owner);

        testQuest.AddQuestEvent(iq);
        testQuest.AddQuestEvent(gr);
        testQuest.AddQuestEvent(cq);

        m_questManager.AddPendingQuest(testQuest);
    }

    private void PopulateKillEnemiesQuest()
    {
        //for debug purposes
        var testQuest = new Quest();
        var interactableObjects = FindObjectsOfType<InteractableEnemy>();
        InteractableEnemy enemy = null;

        foreach (var item in interactableObjects)
        {
            if (item.EnemyName.Equals("Viking"))
            {
                enemy = item;
                break;
            }
        }

        var interactableCharacters = FindObjectsOfType<InteractableCharacter>();
        InteractableCharacter owner = null;

        foreach (var item in interactableCharacters)
        {
            if (item.CharacterName.Equals("Owner"))
            {
                owner = item;
                break;
            }
        }

        var iq = new InvokeQuestEvent(owner);
        var ke = new KillEnemiesQuestEvent(enemy);
        var cq = new CompleteQuestEvent(owner);

        testQuest.AddQuestEvent(iq);
        testQuest.AddQuestEvent(ke);
        testQuest.AddQuestEvent(cq);

        m_questManager.AddPendingQuest(testQuest);
    }

    private void SubscribeToEntityEventBroker()
    {
        EntityEventBroker.OnEntityDeath += WorldEntityDied;
        EntityEventBroker.OnEnemyKilled += WorldEntityDied;
        EntityEventBroker.OnObjectPickUpSuccess += InteractableObjectPicked;
        EntityEventBroker.OnQuestInvoked += InvokeQuest;
        EntityEventBroker.OnQuestCompleted += CompleteQuest;
    }

    public GraphHandler GetGraphHandler()
    {
        return FindObjectOfType<GraphHandler>();
    }

    public int GetQuestCount()
    {
        return m_questManager.GetQuestCount();
    }

    public void WorldEntityDied(WorldEntity invoker, WorldEntity target)
    {
        //Check if target was part of a Quest
        //If yes then QuestManager has to deal with it (async)
        var qe = m_questManager.IsWorldEntityPartOfQuest(target);
        if (qe != null)
        {
            StartCoroutine(m_questManager.ProgressQuest(invoker, qe));
        }

        //Check if QuestGenerator can spawn another quest
    }

    public void InteractableObjectPicked(WorldEntity invoker, InteractableObject interactableObject)
    {
        var qe = m_questManager.IsWorldEntityPartOfQuest(interactableObject);
        if (qe != null)
        {
            StartCoroutine(m_questManager.ProgressQuest(invoker, qe));
        }
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

        // When a Quest is completed try and generate a new one
        m_timePassed = m_timeGoal;
    }

    public void QuestGenerated(Quest quest)
    {
        if (quest == null)
            return;

        m_questManager.AddPendingQuest(quest);
    }
}
