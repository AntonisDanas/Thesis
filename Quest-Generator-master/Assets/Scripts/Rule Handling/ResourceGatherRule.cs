using SideQuestGenerator.GraphHandling;
using SideQuestGenerator.InteractableHandling;
using SideQuestGenerator.QuestHandling;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator.RuleHandling
{
    public class ResourceGatherRule : Rule
    {

        public ResourceGatherRule()
        {
            ruleName = "Resource Gather";
            RuleMultiplier = 1.3f;
        }

        public override List<Graph> ImplementRule(Graph graph)
        {
            List<Graph> returnGraph = new List<Graph>();
            List<Vertex> vertices = graph.GetGraphVertices();
            List<Vertex> resourceVertices = new List<Vertex>();
            List<Vertex> npcVertices = new List<Vertex>();

            foreach (var ver in vertices)
            {
                string label = ver.GetLabel();
                if (label == "Resource")
                    resourceVertices.Add(ver);
                else if (label == "NPC")
                    npcVertices.Add(ver);
            }

            if (resourceVertices.Count == 0)
                return null;

            Vertex resource = resourceVertices[Random.Range(0, resourceVertices.Count)];

            foreach (var npc in npcVertices)
            {
                Graph tempGraph = new Graph();
                tempGraph.AddVertex(npc);
                tempGraph.AddVertex(resource);
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
            InteractableObject resource = null;

            foreach (var c in characters)
            {
                if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(0).GetIndex())
                {
                    invoker = c;
                    break;
                }
            }

            foreach (var o in objects)
            {
                if (o.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(1).GetIndex())
                {
                    resource = o;
                    break;
                }
            }

            int goal = Random.Range(5, 10);
            var iq = new InvokeQuestEvent(invoker, new QuestEventDescription() { DescriptionLabel = "Can you gather " + goal + " of " + resource.ObjectName, ButtonLabel = "Start Quest" });
            var gq = new GatherResourcesQuestEvent(resource, goal);
            var cq = new CompleteQuestEvent(invoker);

            gather.AddQuestEvent(iq);
            gather.AddQuestEvent(gq);
            gather.AddQuestEvent(cq);

            return gather;
        }

        public override float GetAverageCost(Graph graph)
        {
            return 0; // no cost
        }


    }
}