using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
#endif

    [Serializable]
    public class SG_Graph : ScriptableObject
    {
        public string GraphName = "Graph";
        public List<SG_NodeBase> Nodes;
        public List<SG_Edge> Edges;
        public bool InTransition = false;

        public SG_NodeBase transitionStartNode { get; private set; }
        private Vector2 transitionStartCenter;
        public SG_NodeBase transitionEndNode { get; private set; }
        private Vector2 transitionEndCenter;

        private void OnEnable()
        {
            if (Nodes == null) Nodes = new List<SG_NodeBase>();
            if (Edges == null) Edges = new List<SG_Edge>();
        }

        public void InitGraph()
        {
            if (Nodes.Count > 0)
            {
                foreach (var node in Nodes)
                    node.InitNode(node.NodeName);

            }

            if (Edges.Count > 0)
            {
                foreach (var edge in Edges)
                    edge.InitEdge(edge.StartNode, edge.EndNode, edge.Label, edge.ParentGraph);
            }
        }

        public void UpdateGraph()
        {
            if (Nodes.Count > 0)
            {

            }
        }

        public List<SG_Edge> GetAllNodeEdges(SG_NodeBase selectedNode)
        {
            List<SG_Edge> edges = new List<SG_Edge>();

            foreach (var edge in Edges)
            {
                if (edge.StartNode == selectedNode || edge.EndNode == selectedNode)
                    edges.Add(edge);
            }

            return edges;
        }

#if UNITY_EDITOR
        public void UpdateGraphGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            if (InTransition)
            {
                DrawTransitionLine(e.mousePosition);
            }

            if (Edges.Count > 0)
            {
                foreach (var edge in Edges)
                    edge.UpdateEdgeGUI(e, viewRect, viewSkin);
            }

            if (Nodes.Count > 0)
            {
                foreach (var node in Nodes)
                    node.UpdateNodeGUI(e, viewRect, viewSkin);
            }

            ProcessEvents(e, viewRect);

            EditorUtility.SetDirty(this); //It says to Unity Editor to save the scriptale object
        }

        public void StartTransition(SG_NodeBase startingNode)
        {
            transitionStartNode = startingNode;
            transitionStartCenter = startingNode.NodeRect.center;
            InTransition = true;
            transitionEndNode = null; //making sure end node is null
        }

        // returns true if a transition has successfully ended
        public bool EndTransition(SG_NodeBase endingNode)
        {
            InTransition = false;

            if (endingNode == null) return false;
            if (endingNode == transitionStartNode) return false;

            transitionEndNode = endingNode;
            transitionEndCenter = endingNode.NodeRect.center;

            Debug.Log($"Connecting {transitionStartNode.name} with {transitionEndNode.name}");
            return true;
        }

        private void DrawTransitionLine(Vector2 mousePos)
        {
            Handles.BeginGUI();
            Handles.color = Color.white;
            Handles.DrawLine(new Vector3(transitionStartCenter.x, transitionStartCenter.y, 0f),
                            new Vector3(mousePos.x, mousePos.y, 0f));
            Handles.EndGUI();
        }

#endif

        private void ProcessEvents(Event e, Rect viewRect)
        {

        }
    }
}