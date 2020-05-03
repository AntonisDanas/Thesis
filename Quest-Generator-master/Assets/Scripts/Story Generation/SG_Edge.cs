using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SG_Edge : ScriptableObject
{
    public SG_NodeBase StartNode;
    public SG_NodeBase EndNode;
    public string Label;
    public SG_Graph ParentGraph;

    public void InitEdge(SG_NodeBase sNode, SG_NodeBase eNode, string label, SG_Graph pGraph)
    {
        StartNode = sNode;
        EndNode = eNode;
        Label = label;
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
        Handles.DrawLine(StartNode.NodeRect.center,
                        EndNode.NodeRect.center);
        Vector3 labelPos = new Vector3((EndNode.NodeRect.center.x - StartNode.NodeRect.center.x) / 3 + StartNode.NodeRect.center.x,
                                        (EndNode.NodeRect.center.y - StartNode.NodeRect.center.y) / 3 + StartNode.NodeRect.center.y, 0f);

        string labelWithArrow = Label;
        if (StartNode.NodeRect.center.x <= EndNode.NodeRect.center.x)
            labelWithArrow = labelWithArrow + '\u25BA';
        else
            labelWithArrow = '\u25C4' + labelWithArrow;

        Handles.Label(labelPos, labelWithArrow, style);
        Handles.EndGUI();
        EditorUtility.SetDirty(this);
    }
#endif

    //TODO impliment
    private void ProcessEvents(Event e, Rect viewRect)
    {
        return;
    }

}
