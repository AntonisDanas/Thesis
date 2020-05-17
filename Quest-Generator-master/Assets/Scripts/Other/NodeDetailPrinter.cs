using SideQuestGenerator.GraphEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideQuestGenerator
{
    public class NodeDetailPrinter : MonoBehaviour
    {
        public SG_Graph graph;

        // Start is called before the first frame update
        void Start()
        {
            if (graph == null) return;

            foreach (var node in graph.Nodes)
            {
                Debug.Log("Label: " + (node as SG_SpaceNode).Label);
                Debug.Log("Attributes:");
                foreach (var attr in (node as SG_SpaceNode).Attributes)
                {
                    Debug.Log("    " + attr.Key + ": " + attr.Value);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}