/*
 * 
 *  Steal Rule. 
 *  The NPC that wants the item invokes the Quest.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealRule : Rule
{
    public string RuleName { get; private set; }

    public StealRule()
    {
        RuleName = "Steal";
    }

    public override List<Graph> ImplementRule(Graph graph)
    {
        List<Graph> returnGraph = new List<Graph>();
        List<Vertex> vertices = graph.GetGraphVertices();

        foreach (Vertex n2 in vertices)
        {
            if (n2 == null)
                continue;

            List<Vertex> n2Connections = graph.GetOutgoingVerticesByRelationLabel(n2, "Owns");
            if (n2Connections.Count == 0)
                continue;

            Vertex objectOfn2 = n2Connections[Random.Range(0, n2Connections.Count)];

            n2Connections = graph.GetIncomingVerticesByRelationLabels(n2, new List<string>() { "Hates", "Dislikes" }, Condition.OR);

            if (n2Connections.Count == 0)
                continue;

            Vertex enemyOfn2 = n2Connections[Random.Range(0, n2Connections.Count)];


            Graph tempGraph = new Graph();
            tempGraph.AddVertex(enemyOfn2);
            tempGraph.AddVertex(n2);
            tempGraph.AddVertex(objectOfn2);
            tempGraph.SetRelation(enemyOfn2, n2, "Hates");
            tempGraph.SetRelation(n2, objectOfn2, "Owns");
            returnGraph.Add(tempGraph);
        }


        return returnGraph;
    }

    public override Quest GenerateQuestFromRule(Graph graph, List<InteractableCharacter> characters, List<InteractableObject> objects, List<InteractableEnemy> enemies)
    {
        if (graph == null)
            return null;

        Quest steal = new Quest();

        InteractableCharacter enemy = null;
        InteractableCharacter n2 = null;
        InteractableObject obj = null;

        foreach (var c in characters)
        {
            if (enemy != null && n2 != null)
                break;


            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(0).GetIndex())
                enemy = c;

            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(1).GetIndex())
                n2 = c;
        }

        foreach (var c in objects)
        {
            if (c.GetIndexOfGraphInstance() == graph.GetVertexAtPosition(2).GetIndex())
            {
                obj = c;
                break;
            }
        }

        var iq = new InvokeQuestEvent(enemy);
        var sq = new StealQuestEvent(obj);
        var tq = new TransferObjectQuestEvent(enemy, obj);
        var cq = new CompleteQuestEvent(enemy);

        steal.AddQuestEvent(iq);
        steal.AddQuestEvent(sq);
        steal.AddQuestEvent(tq);
        steal.AddQuestEvent(cq);

        return steal;
    }
}
