using SideQuestGenerator.QuestHandling;
using SideQuestGenerator.GraphHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SideQuestGenerator.RuleHandling;
using SideQuestGenerator.InteractableHandling;

namespace SideQuestGenerator.Scheduling
{
    public class QuestGenerator
    {
        private QuestScheduler m_questScheduler;
        private GraphHandler m_graphHandler;
        private int m_questPoolSize;
        private float m_questPoolGenerationMultiplier;
        private float m_ruleCostMultiplier;
        private float m_ruleCountMultiplier;

        private List<InteractableCharacter> m_interactableCharacters;
        private List<InteractableObject> m_interactableObjects;
        private List<InteractableEnemy> m_interactableEnemies;

        private Dictionary<string, Rule> m_availableRules;
        private Dictionary<string, int> m_ruleOccurenceCount;


        public QuestGenerator(QuestScheduler qs, GraphHandler gh, int qps, float qpgm, float costM, float countM)
        {
            m_questScheduler = qs;
            m_graphHandler = gh;
            m_questPoolSize = qps;
            m_questPoolGenerationMultiplier = qpgm;
            m_ruleCostMultiplier = costM;
            m_ruleCountMultiplier = countM;

            m_interactableCharacters = new List<InteractableCharacter>();
            m_interactableObjects = new List<InteractableObject>();
            m_interactableEnemies = new List<InteractableEnemy>();

            m_availableRules = new Dictionary<string, Rule>();
            m_ruleOccurenceCount = new Dictionary<string, int>();

            EntityEventBroker.OnEntityEnroll += EnrollEntity;

            InstantiateRules();
        }

        public IEnumerator GenerateNewQuest()
        {
            if (m_graphHandler == null)
                m_questScheduler.GetGraphHandler();

            // Create Quest depending on Quest Pool Size
            float randomNum = UnityEngine.Random.Range(0f, m_questPoolSize);
            randomNum *= m_questPoolGenerationMultiplier;
            int currentQuestCount = m_questScheduler.GetQuestCount();

            if (randomNum < currentQuestCount)
            {
                Debug.Log("Cannot create Quest because of pool size");
                m_questScheduler.QuestGenerated(null);
                yield break;
            }

            // Choose Best Rule
            Rule r = ChooseBestRule();

            if (r == null)
            {
                Debug.Log("No best rule available");
                m_questScheduler.QuestGenerated(null);
                yield break;
            }

            // Search world graph to apply rule
            // If no match found find other rule
            List<Graph> returnGraphs = m_graphHandler.SearchForPattern(r);

            if (returnGraphs == null || returnGraphs.Count == 0)
            {
                Debug.Log("No available characters for the pattern");
                m_questScheduler.QuestGenerated(null);
                yield break;
            }

            // Select starting character

            //1) Check if character already has quest (create new graph list)
            if (!CheckIfCharactersHaveQuest(ref returnGraphs, m_interactableCharacters))
            {
                Debug.Log("All available characters have a Quest");
                m_questScheduler.QuestGenerated(null);
                yield break;
            }

            //2) Rearrange list according to relationship with Player
            Graph final = ReturnPatternAccordingToRelationship(returnGraphs);

            if (final == null)
            {
                Debug.Log("Characters could not give quest");
                m_questScheduler.QuestGenerated(null);
                yield break;
            }

            //3) Generate quest (implement inside Rule)
            Quest quest = r.GenerateQuestFromRule(final, m_interactableCharacters, m_interactableObjects, m_interactableEnemies);

            // Send quest to QuestScheduler
            m_questScheduler.QuestGenerated(quest);
            yield return null;
        }

        private void InstantiateRules()
        {
            Rule steal = new StealRule();
            Rule loveMurder = new LoveMurderRule();
            Rule resourceGather = new ResourceGatherRule();
            Rule killEnemy = new EnemyKillRule();

            m_availableRules.Add(steal.GetRuleName(), steal);
            m_ruleOccurenceCount.Add(steal.GetRuleName(), 0);

            m_availableRules.Add(loveMurder.GetRuleName(), loveMurder);
            m_ruleOccurenceCount.Add(loveMurder.GetRuleName(), 0);

            m_availableRules.Add(resourceGather.GetRuleName(), resourceGather);
            m_ruleOccurenceCount.Add(resourceGather.GetRuleName(), 0);

            m_availableRules.Add(killEnemy.GetRuleName(), killEnemy);
            m_ruleOccurenceCount.Add(killEnemy.GetRuleName(), 0);
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

        private Rule ChooseBestRule()
        {
            float thres = 0f;

            Rule selectedRule = null;

            foreach (var rule in m_availableRules)
            {
                float temp = m_graphHandler.GetRuleCost(rule.Value);

                float cost = 1 / (1 + temp);
                int count = 1 / (1 + m_ruleOccurenceCount[rule.Key]);
                float multiplier = rule.Value.RuleMultiplier;

                float fri = (m_ruleCostMultiplier * cost + m_ruleCountMultiplier * count) * multiplier;
                fri *= UnityEngine.Random.Range(0f, 1f); // just a random variable

                if (fri > thres)
                {
                    selectedRule = rule.Value;
                    thres = fri;
                }
            }

            Debug.Log("Rule Chosen: " + selectedRule.GetRuleName());

            return selectedRule;
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

                    if (ic.HasQuestEvent())
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

            for (int i = 7; i > 0; i--)
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

}
