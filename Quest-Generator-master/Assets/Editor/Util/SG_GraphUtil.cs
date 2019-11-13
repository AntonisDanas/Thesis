using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SG_GraphUtil
{
    public static void CreateNewGraph(string graphName)
    {
        SG_Graph curGraph = ScriptableObject.CreateInstance<SG_Graph>();

        if (curGraph == null)
        {
            EditorUtility.DisplayDialog("Graph Message:", "Unable to create new graph", "OK");
            return;
        }

        curGraph.GraphName = graphName;
        curGraph.InitGraph();

        AssetDatabase.CreateAsset(curGraph, "Assets/Database/" + graphName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        SG_GraphEditorWindow graphEditor = EditorWindow.GetWindow<SG_GraphEditorWindow>();
        if (graphEditor != null)
            graphEditor.curGraph = curGraph;  // assign the current graph to graph work view 
    }

    public static void UnloadGraph()
    {
        SG_GraphEditorWindow graphEditor = EditorWindow.GetWindow<SG_GraphEditorWindow>(); // this gets the instance of the active window

        if (graphEditor != null)
            graphEditor.curGraph = null;
    }

    public static void LoadGraph()
    {
        SG_Graph curGraph = null;
        string graphPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + "/Database", "");

        int appPathLen = Application.dataPath.Length;
        string finalPath = graphPath.Substring(appPathLen - "Assets".Length);

        curGraph = (SG_Graph)AssetDatabase.LoadAssetAtPath(finalPath, typeof(SG_Graph));

        if (curGraph == null)
        {
            EditorUtility.DisplayDialog("Graph Message", "Unable to load selected graph", "OK");
            return;
        }

        SG_GraphEditorWindow curWindow = EditorWindow.GetWindow<SG_GraphEditorWindow>();

        curWindow.curGraph = curWindow != null ? curGraph : null;
    }

    public static void CreateNode(SG_Graph graph, Vector2 mousePos, string nodeName, List<string> labels)
    {
        if (graph == null) return;

        SG_SpaceNode curNode = null;
        curNode = ScriptableObject.CreateInstance<SG_SpaceNode>();

        if (curNode == null) return;

        curNode.InitNode(nodeName);
        curNode.Labels = labels;
        curNode.name = "n_" + nodeName.Replace(" ", "");
        curNode.NodeRect.x = mousePos.x;
        curNode.NodeRect.y = mousePos.y;
        curNode.ParentGraph = graph;
        graph.Nodes.Add(curNode);

        AssetDatabase.AddObjectToAsset(curNode, graph); // adds node to the asset
        AssetDatabase.SaveAssets(); //then saves the node
        AssetDatabase.Refresh();
    }

    public static void CreateEdge(SG_Graph graph, SG_NodeBase startNode, SG_NodeBase endNode, string label)
    {
        if (graph == null ||
            startNode == null ||
            endNode == null) return;

        SG_Edge curEdge = null;
        curEdge = ScriptableObject.CreateInstance<SG_Edge>();

        if (curEdge == null) return;

        curEdge.name = "e_" + startNode.NodeName.Replace(" ", "") + label + endNode.NodeName.Replace(" ", "");

        curEdge.InitEdge(startNode, endNode, label, graph);
        graph.Edges.Add(curEdge);

        AssetDatabase.AddObjectToAsset(curEdge, graph); // adds edge to the asset
        AssetDatabase.SaveAssets(); //then saves the edge
        AssetDatabase.Refresh();
    }
}
