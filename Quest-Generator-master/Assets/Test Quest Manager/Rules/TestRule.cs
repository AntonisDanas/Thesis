using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRule : Rule
{
    public override List<Graph> ImplementRule(Graph graph)
    {
        List<Graph> returnGraph = new List<Graph>();
        List<Vertex> vertices = graph.GetGraphVertices();

        foreach (Vertex vertex in vertices)
        {
            if (vertex != null)
            {
                List<Vertex> destVertices = graph.GetOutgoingVerticesByRelationLabel(vertex, "Hates");

                foreach (Vertex vertex1 in destVertices)
                {
                    List<Vertex> destVertices2 = graph.GetOutgoingVerticesByRelationLabel(vertex1, "Hates");

                    foreach (Vertex vertex2 in destVertices2)
                    {
                        if (vertex2.Equals(vertex))
                            continue;
                        
                        Graph tempGraph = new Graph();
                        tempGraph.AddVertex(vertex);
                        tempGraph.AddVertex(vertex1);
                        tempGraph.AddVertex(vertex2);
                        tempGraph.SetRelation(vertex, vertex1, "Hates");
                        tempGraph.SetRelation(vertex1, vertex2, "Hates");
                        returnGraph.Add(tempGraph);
                    }

                }
            }
        }


        return returnGraph;
    }

    public override Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies)
    {
        if (graph == null)
            return null;

        Quest testQuest = new Quest();

        InteractableCharacter ic1 = null;
        InteractableCharacter ic2 = null;
        InteractableCharacter ic3 = null;

        foreach (var c in characters)
        {
            if (ic1 != null && ic2 != null && ic3 != null)
                break;


            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(0).GetIndex())
                ic1 = c;

            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(1).GetIndex())
                ic2 = c;

            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(2).GetIndex())
                ic3 = c;
        }

        var iq = new InvokeQuestEvent(ic1);
        var aq = new AssassinationQuestEvent(ic2);
        var cq = new CompleteQuestEvent(ic3);

        testQuest.AddQuestEvent(iq);
        testQuest.AddQuestEvent(aq);
        testQuest.AddQuestEvent(cq);

        return testQuest;
    }
}
