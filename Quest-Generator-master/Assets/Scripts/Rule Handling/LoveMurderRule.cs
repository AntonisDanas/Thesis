/*
 * 
 *  LoveMurder Rule. 
 *  The Lover invokes the Quest.
 * 
 */


using SideQuestGenerator.GraphHandling;
using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.QuestHandling;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator.RuleHandling
{
    public class LoveMurderRule : Rule
    {

        public LoveMurderRule()
        {
            ruleName = "Love Murder";
            RuleMultiplier = 0.8f;
        }

        public override List<Graph> ImplementRule(Graph graph)
        {
            List<Graph> returnGraph = new List<Graph>();
            List<Vertex> vertices = graph.GetGraphVertices();

            foreach (Vertex n2 in vertices)
            {
                if (n2 == null)
                    continue;

                List<Vertex> n2Connections = graph.GetOutgoingVerticesByRelationReason(n2, "Loves", "Affair");
                if (n2Connections.Count == 0)
                    continue;

                Vertex loverOfn2 = n2Connections[Random.Range(0, n2Connections.Count)];

                n2Connections = graph.GetOutgoingVerticesByRelationLabels(n2, new List<string>() { "Hates", "Married" }, Condition.AND);

                // it should return 1 vertex
                if (n2Connections.Count != 1)
                    continue;

                Vertex husbantOfn2 = n2Connections[0];

                Graph tempGraph = new Graph();
                tempGraph.AddVertex(loverOfn2);
                tempGraph.AddVertex(n2);
                tempGraph.AddVertex(husbantOfn2);
                tempGraph.SetRelation(n2, loverOfn2, "Loves");
                tempGraph.SetRelation(n2, husbantOfn2, "Hates");
                returnGraph.Add(tempGraph);
            }


            return returnGraph;
        }

        public override Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies)
        {
            if (graph == null)
                return null;

            Quest loveMurder = new Quest();

            InteractableCharacter lover = null;
            InteractableCharacter husbant = null;

            foreach (var c in characters)
            {
                if (lover != null && husbant != null)
                    break;


                if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(0).GetIndex())
                    lover = c;

                if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(2).GetIndex())
                    husbant = c;
            }

            var iq = new InvokeQuestEvent(lover, new QuestEventDescription() { DescriptionLabel = "Please kill " + husbant.CharacterName, ButtonLabel = "Start Quest" });
            var aq = new AssassinationQuestEvent(husbant);
            var cq = new CompleteQuestEvent(lover);

            loveMurder.AddQuestEvent(iq);
            loveMurder.AddQuestEvent(aq);
            loveMurder.AddQuestEvent(cq);

            return loveMurder;
        }

        public override float GetAverageCost(Graph graph)
        {
            var results = ImplementRule(graph);
            float cost = 0f;
            float count = 0f;

            foreach (var result in results)
            {
                Vertex husbant = graph.GetVertexAtPosition(2);
                cost += graph.GetIncomingVerticesByRelationLabels(husbant, new List<string>() { "Hates", "Dislikes", "Likes", "Loves" }, Condition.OR).Count;
                count++;
            }

            return count == 0 ? 0f : cost / count;
        }
    }
}