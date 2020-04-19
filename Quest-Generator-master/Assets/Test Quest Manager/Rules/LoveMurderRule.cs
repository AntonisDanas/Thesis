﻿/*
 * 
 *  LoveMurder Rule. 
 *  The Lover invokes the Quest.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveMurderRule : Rule
{
    public string RuleName { get; private set; }

    public LoveMurderRule()
    {
        RuleName = "Love Murder";
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

            Vertex hosbantOfn2 = n2Connections[0];

            Graph tempGraph = new Graph();
            tempGraph.AddVertex(loverOfn2);
            tempGraph.AddVertex(n2);
            tempGraph.AddVertex(hosbantOfn2);
            tempGraph.SetRelation(n2, loverOfn2, "Loves");
            tempGraph.SetRelation(n2, hosbantOfn2, "Hates");
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

        var iq = new InvokeQuestEvent(lover);
        var aq = new AssassinationQuestEvent(husbant);
        var cq = new CompleteQuestEvent(lover);

        loveMurder.AddQuestEvent(iq);
        loveMurder.AddQuestEvent(aq);
        loveMurder.AddQuestEvent(cq);

        return loveMurder;
    }

}