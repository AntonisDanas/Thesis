using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
#endif

    [Serializable]
    public class SG_Edge : ScriptableObject
    {
        public SG_NodeBase StartNode;
        public SG_NodeBase EndNode;
        public string Label;
        public string Reason;
        public SG_Graph ParentGraph;

        public void InitEdge(SG_NodeBase sNode, SG_NodeBase eNode, string label, string reason, SG_Graph pGraph)
        {
            StartNode = sNode;
            EndNode = eNode;
            Label = label;
            Reason = reason;
            ParentGraph = pGraph;
        }

#if UNITY_EDITOR
        public void UpdateEdgeGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            ProcessEvents(e, viewRect);

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;

            Handles.BeginGUI();
            Handles.color = Color.white;
            Handles.DrawLine(StartNode.nodeRect.center,
                            EndNode.nodeRect.center);
            Vector3 labelPos = new Vector3((EndNode.nodeRect.center.x - StartNode.nodeRect.center.x) / 3 + StartNode.nodeRect.center.x,
                                            (EndNode.nodeRect.center.y - StartNode.nodeRect.center.y) / 3 + StartNode.nodeRect.center.y, 0f);

            string labelWithArrow = Label;
            if (StartNode.nodeRect.center.x <= EndNode.nodeRect.center.x)
                labelWithArrow = labelWithArrow + ": " + "{" + Reason + "} " + '\u25BA';
            else
                labelWithArrow = '\u25C4' + labelWithArrow + ": " + "{" + Reason + "} ";

            Handles.Label(labelPos, labelWithArrow, style);
            Handles.EndGUI();
            EditorUtility.SetDirty(this);
        }

        public override string ToString()
        {
            return $"({StartNode.nodeName})-[{Label}: {{{Reason}}}]->({EndNode.nodeName})";
        }


        //TODO impliment
        private void ProcessEvents(Event e, Rect viewRect)
        {
            return;
        }
#endif
    }
}