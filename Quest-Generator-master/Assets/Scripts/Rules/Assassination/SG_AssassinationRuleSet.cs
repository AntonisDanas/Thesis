using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_AssassinationRuleSet : SG_RuleSet
{
    private SG_SpaceNode invoker;
    private SG_SpaceNode target;
    private SG_Graph spaceGraph;

    public override void SearchSpaceGraph(SG_Graph graph, SG_SpaceNode startNode)
    {
        List<SG_SpaceNode> endNodes = new List<SG_SpaceNode>();
        // search graph for start node
        foreach (var edge in graph.Edges)
        {
            if (edge.StartNode == startNode &&
                edge.Label.Equals("Hates"))
            {
                endNodes.Add((SG_SpaceNode)edge.EndNode);
            }
        }

        //TODO impliment selection system for quest
        // for now it's just random
        if (endNodes.Count == 0) return;

        int selection = Random.Range(0, endNodes.Count);

        invoker = startNode;
        target = endNodes[selection];
        spaceGraph = graph;
    }

    public override void Greet()
    {
        // Debug.Log("Assassination Rule");
    }

    public override void ExecuteRule()
    {
        if (invoker == null || target == null) return;
    }
}
