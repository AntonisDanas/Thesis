using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public static class SG_GraphUtils 
{
	public static void CreateNewGraph(string graphName)
	{
		Debug.Log ("Creating new Graph!");
		SG_Graph curGraph;
		SG_GraphEditorWindow curWindow = (SG_GraphEditorWindow)EditorWindow.GetWindow<SG_GraphEditorWindow> ();
		if(curWindow != null)
		{
			curGraph = CreateGraphObject(graphName);
			curWindow.workView.curGraph = curGraph;
			curWindow.workView.SetPropertyView(curGraph);
		}
	}

	public static SG_Graph LoadGraph()
	{
		SG_Graph curGraph = null;
		string graphPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + "/GT_NodeEditor/Database", "");
		int appPathLen = Application.dataPath.Length;
		string finalPath = graphPath.Substring (appPathLen-6);

		curGraph = (SG_Graph)AssetDatabase.LoadAssetAtPath (finalPath, typeof(SG_Graph));
		return curGraph;
	}

	static SG_Graph CreateGraphObject(string graphName)
	{
		SG_Graph curGraph = ScriptableObject.CreateInstance<SG_Graph> ();
		if(curGraph != null)
		{
			curGraph.graphName = graphName;
			AssetDatabase.CreateAsset(curGraph, "Assets/GT_NodeEditor/Database/" + graphName + ".asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return curGraph;
		}
		else
		{
			return null;
		}
	}

	public static void CreateNode(SG_Graph curGraph, NodeType nodeType, Vector2 nodePos, string nodeName = "", StringObjectDictionary attributes = null)
	{
		if(curGraph != null)
		{
			SG_NodeBase curNode = null;
			switch(nodeType)
			{
				case NodeType.NPC:
					curNode = ScriptableObject.CreateInstance<SG_NPCNode>();
					break;
				case NodeType.Enemy:
					curNode = ScriptableObject.CreateInstance<SG_EnemyNode>();
					break;
				case NodeType.Resource:
					curNode = ScriptableObject.CreateInstance<SG_ResourceNode>();
					break;
				case NodeType.Object:
					curNode = ScriptableObject.CreateInstance<SG_ObjectNode>();
					break;
				default:
					break;
			}

			if(curNode != null)
			{
				curNode.InitNode();
				curNode.nodeRect.x = nodePos.x;
				curNode.nodeRect.y = nodePos.y;
				curNode.parentGraph = curGraph;

				if (nodeName != "")
					curNode.name = "n_" + nodeName.Replace(" ", "");

				curNode.nodeName = nodeName;

				switch (nodeType)
				{
					case NodeType.NPC:
						(curNode as SG_NPCNode).Attributes = attributes;
						break;
					case NodeType.Enemy:
						(curNode as SG_EnemyNode).Attributes = attributes;
						break;
					case NodeType.Resource:
						(curNode as SG_ResourceNode).Attributes = attributes;
						break;
					case NodeType.Object:
						(curNode as SG_ObjectNode).Attributes = attributes;
						break;
					default:
						break;
				}


				curGraph.nodes.Add(curNode);

				AssetDatabase.AddObjectToAsset(curNode, curGraph);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}
	}

	public static void CreateEdge(SG_Graph curGraph, SG_NodeBase startNode, SG_NodeBase endNode, string label, string reason)
	{
		SG_Edge edge = ScriptableObject.CreateInstance<SG_Edge>();

		edge.InitEdge(startNode, endNode, label, reason, curGraph);

		edge.name = "e_" + startNode.nodeName.Replace(" ", "") + "_" + label + reason + "_" + endNode.nodeName.Replace(" ", "");

		curGraph.edges.Add(edge);
		AssetDatabase.AddObjectToAsset(edge, curGraph);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	public static void DeleteNode(int nodeID, SG_Graph curGraph)
	{
		if(curGraph != null)
		{
			if(curGraph.nodes.Count >= nodeID)
			{
				SG_NodeBase deleteNode = curGraph.nodes[nodeID];
				if(deleteNode != null)
				{
					if (deleteNode == curGraph.selectedNode)
					{
						curGraph.selectedNode = null;
						curGraph.showProperties = false;
					}



					curGraph.nodes.RemoveAt(nodeID);
					GameObject.DestroyImmediate(deleteNode, true);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}
	}

	public static void DrawEditorGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor)
	{
		//Process the Grid spacing values
		int widthDivs = Mathf.CeilToInt (viewRect.width / gridSpacing);
		int heightDivs = Mathf.CeilToInt (viewRect.height / gridSpacing);
		Handles.BeginGUI ();
		Handles.color = new Color (gridColor.r, gridColor.g, gridColor.b, gridOpacity);

		for (int x = 0; x < widthDivs; x++) 
		{
			Handles.DrawLine (new Vector3 ((gridSpacing * x), 0f, 0f), new Vector3 ((gridSpacing * x), viewRect.height, 0f));
		}

		for (int y = 0; y < heightDivs; y++) 
		{
			Handles.DrawLine (new Vector3 (0f, (gridSpacing * y), 0f), new Vector3 (viewRect.width, (gridSpacing * y), 0f));
		}

		Handles.color = Color.white;
		Handles.EndGUI ();
		
	}
}
