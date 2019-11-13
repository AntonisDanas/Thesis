using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_StealRuleSet : SG_RuleSet
{
    public override void SearchSpaceGraph(SG_Graph graph, SG_SpaceNode startNode)
    {
        // search graph for start node
        foreach (var edge in graph.Edges)
        {
            if (edge.StartNode == startNode)
            {
                Debug.Log(edge.StartNode.NodeName + " -Steal-> " + edge.EndNode.NodeName);
            }
        }
    }
    public override void Greet()
    {
        // Debug.Log("Steal Rule");
    }

    public override void ExecuteRule()
    {
        throw new System.NotImplementedException();
    }
}
