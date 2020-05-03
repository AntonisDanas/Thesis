using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SG_GraphWorkView : SG_ViewBase
{
    private Vector2 mousePos;
    private SG_NodeBase selectedNode = null;

    public SG_GraphWorkView() : base("Graph Work View") { }

    public override void UpdateView(Rect editorRect, Rect percentageRect, SG_Graph curGraph)
    {
        base.UpdateView(editorRect, percentageRect, curGraph);

        if (curGraph != null) ViewTitle = curGraph.GraphName;
        else ViewTitle = "No graph";

        GUI.Box(ViewRect, ViewTitle, viewSkin.GetStyle("WorkViewBG"));

        GUILayout.BeginArea(ViewRect);

        if (curGraph != null)
        {
            Event e = Event.current;
            curGraph.UpdateGraphGUI(e, editorRect, viewSkin);
        }
        
        GUILayout.EndArea();
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);

        mousePos = e.mousePosition;

        if (!ViewRect.Contains(mousePos)) return;

        // check if in transition to end it
        if (e.type == EventType.MouseDown &&
            curGraph != null &&
            curGraph.InTransition)
        {
            if (curGraph.EndTransition(CheckIfMouseOverNode()))
                SG_EdgePopupWindow.InitEdgePopup(curGraph, 
                                                curGraph.transitionStartNode, 
                                                curGraph.transitionEndNode);

            return;
        }

        if (e.button == 1)
        {
            ProcessContextMenu(e);
        }
    }

    private void ProcessContextMenu(Event e)
    {
        selectedNode = CheckIfMouseOverNode();

        GenericMenu menu = new GenericMenu();

        if (selectedNode != null)
        {
            menu.AddItem(new GUIContent("Delete Node"), false, NodeCallback, "DeleteNode");
            menu.AddItem(new GUIContent("Make Transition"), false, NodeCallback, "MakeTransition");

            menu.ShowAsContext();
            return;
        }

        menu.AddItem(new GUIContent("Create Graph"), false, ContextCallback, "CreateGraph");
        menu.AddItem(new GUIContent("Load Graph"), false, ContextCallback, "LoadGraph");

        if (curGraph != null)
        {
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Unload Graph"), false, ContextCallback, "UnloadGraph");

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "AddNode");
        }

        menu.ShowAsContext();

        //e.Use();
    }

    private void ContextCallback(object obj)
    {
        switch (obj.ToString())
        {
            case "CreateGraph":
                SG_GraphPopupWindow.InitGraphPopup();
                break;
            case "LoadGraph":
                SG_GraphUtil.LoadGraph();
                break;
            case "UnloadGraph":
                SG_GraphUtil.UnloadGraph();
                break;
            case "AddNode":
                SG_NodePopupWindow.InitNodePopup(curGraph, mousePos);
                break;
            default:
                break;
        }
    }

    private void NodeCallback(object obj)
    {
        switch (obj.ToString())
        {
            case "DeleteNode":
                SG_GraphUtil.DeleteNode(curGraph, selectedNode);
                break;
            case "MakeTransition":
                CreateTransition();
                break;
            default:
                break;
        }
    }

    private SG_NodeBase CheckIfMouseOverNode()
    {
        if (curGraph != null)
        {
            foreach (var node in curGraph.Nodes)
            {
                if (node.NodeRect.Contains(mousePos))
                {
                    return node;
                }
            }
        }
        
        return null;
    }

    private void CreateTransition()
    {
        if (curGraph == null || curGraph.InTransition) return;

        curGraph.StartTransition(selectedNode);
    }
}
