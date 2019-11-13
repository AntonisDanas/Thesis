using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_SideQuestScheduler : MonoBehaviour
{
    public SG_Graph SpaceGraph;
    public List<SG_RuleSet> Rules;
    public List<SG_SideQuest> ActiveQuests;

    // Start is called before the first frame update
    void Start()
    {
        if (Rules.Count > 0)
        {
            foreach (var rule in Rules)
            {
                rule.Greet();
            }

            Rules[0].SearchSpaceGraph(SpaceGraph, (SG_SpaceNode)SpaceGraph.Nodes[0]);
            Rules[0].ExecuteRule();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
