using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SG_RuleSet : MonoBehaviour
{
    public abstract void SearchSpaceGraph(SG_Graph graph, SG_SpaceNode startNode);
    public abstract void ExecuteRule();
    public abstract void Greet();
}
