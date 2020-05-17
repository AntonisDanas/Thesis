using SideQuestGenerator.GraphHandling;
using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.QuestHandling;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator.RuleHandling
{
    public class EnemyKillRule : Rule
    {

        public EnemyKillRule()
        {
            ruleName = "Enemy Kill";
            RuleMultiplier = 1.2f;
        }

        public override List<Graph> ImplementRule(Graph graph)
        {
            List<Graph> returnGraph = new List<Graph>();
            List<Vertex> vertices = graph.GetGraphVertices();
            List<Vertex> enemyVertices = new List<Vertex>();
            List<Vertex> npcVertices = new List<Vertex>();

            foreach (var ver in vertices)
            {
                string label = ver.GetLabel();
                if (label == "Enemy")
                    enemyVertices.Add(ver);
                else if (label == "NPC")
                    npcVertices.Add(ver);
            }

            if (enemyVertices.Count == 0)
                return null;

            Vertex enemy = enemyVertices[Random.Range(0, enemyVertices.Count)];

            foreach (var npc in npcVertices)
            {
                Graph tempGraph = new Graph();
                tempGraph.AddVertex(npc);
                tempGraph.AddVertex(enemy);
                returnGraph.Add(tempGraph);
            }

            return returnGraph;
        }

        public override Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies)
        {
            if (graph == null)
                return null;

            Quest gather = new Quest();

            InteractableCharacter invoker = null;
            InteractableEnemy enemy = null;

            foreach (var c in characters)
            {
                if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(0).GetIndex())
                {
                    invoker = c;
                    break;
                }
            }

            foreach (var e in enemies)
            {
                if (e.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(1).GetIndex())
                {
                    enemy = e;
                    break;
                }
            }

            int goal = Random.Range(3, 10);
            var iq = new InvokeQuestEvent(invoker, new QuestEventDescription() { DescriptionLabel = "Can you kill " + goal + " of " + enemy.EnemyName, ButtonLabel = "Start Quest" });
            var kq = new KillEnemiesQuestEvent(enemy, goal);
            var cq = new CompleteQuestEvent(invoker);

            gather.AddQuestEvent(iq);
            gather.AddQuestEvent(kq);
            gather.AddQuestEvent(cq);

            return gather;
        }

        public override float GetAverageCost(Graph graph)
        {
            return 0; // no cost
        }
    }
}