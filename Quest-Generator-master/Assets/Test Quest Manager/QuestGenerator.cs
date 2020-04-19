using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator
{
    private QuestScheduler m_questScheduler;
    private GraphHandler m_graphHandler;
    private List<InteractableCharacter> m_interactableCharacters;
    private List<InteractableObject> m_interactableObjects;
    private List<InteractableEnemy> m_interactableEnemies;


    public QuestGenerator(QuestScheduler qs, GraphHandler gh)
    {
        m_questScheduler = qs;
        m_graphHandler = gh;
        m_interactableCharacters = new List<InteractableCharacter>();
        m_interactableObjects = new List<InteractableObject>();
        m_interactableEnemies = new List<InteractableEnemy>();

        EntityEventBroker.OnEntityEnroll += EnrollEntity;
    }

    public IEnumerator GenerateNewQuest()
    {
        if (m_graphHandler == null)
            m_questScheduler.GetGraphHandler();


        // Choose Best Rule
        StealRule r = new StealRule();

        // Search world graph to apply rule
        // If no match found find other rule
        List<Graph> returnGraphs = m_graphHandler.SearchForPattern(r);

        if (returnGraphs.Count == 0)
        {
            Debug.Log("No available characters for the pattern");
            m_questScheduler.QuestGenerated(null);
            yield break;
        }

        // Select starting character

        //1) Check if character already has quest (create new graph list)
        if (!CheckIfCharactersHaveQuest(ref returnGraphs, m_interactableCharacters))
        {
            // All characters have quests

            Debug.Log("All available characters have a Quest");
            m_questScheduler.QuestGenerated(null);
            yield break;
        }

        //2) Rearrange list according to relationship with Player
        //Graph final = ReturnPatternAccordingToRelationship(returnGraphs);
        Graph final = returnGraphs[0];

        //if (final == null)
        //{
        //    // Characters could not give quest

        //    m_questScheduler.QuestGenerated(null);
        //    yield break;
        //}

        //3) Generate quest (implement inside Rule)
        Quest quest = r.GenerateQuestFromRule(final, m_interactableCharacters, m_interactableObjects, m_interactableEnemies);

        // Send quest to QuestScheduler
        m_questScheduler.QuestGenerated(quest);
        yield return null;
    }

    private void EnrollEntity(WorldEntity entity)
    {
        if (entity is InteractableCharacter)
            m_interactableCharacters.Add(entity as InteractableCharacter);
        else if (entity is InteractableObject)
            m_interactableObjects.Add(entity as InteractableObject);
        else if (entity is InteractableEnemy)
            m_interactableEnemies.Add(entity as InteractableEnemy);
    }

    private bool CheckIfCharactersHaveQuest(ref List<Graph> returnGraphs, List<InteractableCharacter> characters)
    {
        var tempGraph = new List<Graph>(returnGraphs);

        foreach (Graph graph in tempGraph)
        {
            var startVerIndex = graph.GetVertexAtPosition(0).GetIndex();

            foreach (var ic in m_interactableCharacters)
            {
                if (startVerIndex != ic.GetIndexOfGraphInstance())
                    continue;

                if (ic.HasQuestAvailable())
                    returnGraphs.Remove(graph);

                break;
            }
        }

        return returnGraphs.Count > 0;
    }

    private Graph ReturnPatternAccordingToRelationship(List<Graph> graphs)
    {
        int playerIndex = m_graphHandler.GetPlayerIndex();
        if (playerIndex == -1) // Player doesn't exist
            return null;

        // Get relationship between player and first node
        Dictionary<int, List<Graph>> graphsByRelationship = new Dictionary<int, List<Graph>>();
        foreach (var item in graphs)
        {
            int targetIndex = item.GetVertexAtPosition(0).GetIndex();
            string relationship = m_graphHandler.GetRelationshipLabelBetweenNodes(targetIndex, playerIndex);

            Debug.Log(relationship);

            if (relationship == "")
                continue;

            RelationMultiplier rm;

            if (!Enum.TryParse(relationship, out rm))
                continue;

            if (graphsByRelationship.ContainsKey((int)rm))
                graphsByRelationship[(int)rm].Add(item);
            else
                graphsByRelationship.Add((int)rm, new List<Graph>() { item });
        }

        // No valid relationships
        if (graphsByRelationship.Keys.Count == 0)
            return null;

        for (int i = 7; i > 0 ; i--)
        {
            if (!graphsByRelationship.ContainsKey(i))
                continue;

            List<Graph> finalList = graphsByRelationship[i];
            int ran = UnityEngine.Random.Range(1, 7);

            if (i < ran)  // Randomly choose if we'll give a quest depending on RelationshipMultiplier
                continue;

            ran = UnityEngine.Random.Range(0, finalList.Count);

            Debug.Log("Quest Given!");

            return finalList[ran];
        }

        Debug.Log("Quest Not Given!");

        return null;
    }
    
}

public enum RelationMultiplier
{
    Hates = 1,
    Dislikes = 2,
    Distrusts = 3,
    Neutral = 4,
    Trusts = 5,
    Likes = 6,
    Loves = 7
}