using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace SideQuestGenerator.GraphEditor
{
	[Serializable]
	public class SG_Graph : ScriptableObject
	{
		#region Public Variables
		public bool wantsConnection = false;
		public string graphName = "NewGraph";
		public List<SG_NodeBase> nodes;
		public List<SG_Edge> edges;

		//create list for edges

		public SG_NodeBase selectedNode;
		public SG_NodeBase connectionNode;
		public bool showProperties = false;
		#endregion

		#region Private Variables
		#endregion

		#region Main Methods
		void OnEnable()
		{
			if (nodes == null)
			{
				nodes = new List<SG_NodeBase>();
			}

			if (edges == null)
			{
				edges = new List<SG_Edge>();
			}
		}

		public void InitGraph()
		{
			if (nodes.Count > 0)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					nodes[i].InitNode();
				}
			}

			if (edges.Count > 0)
			{
				foreach (var edge in edges)
					edge.InitEdge(edge.StartNode, edge.EndNode, edge.Label, edge.Reason, edge.ParentGraph);
			}
		}

		public void UpdateGraph(Rect viewRect, Event e, GUISkin editorSkin)
		{
			if (nodes.Count > 0)
			{
				ProcessEvents(e, viewRect);

				if (wantsConnection)
				{
					if (connectionNode != null)
					{
						DrawConnectionToMouse(e.mousePosition);
					}
				}

				for (int i = 0; i < nodes.Count; i++)
				{
					nodes[i].UpdateNode(viewRect, e, editorSkin);
				}

				for (int i = 0; i < edges.Count; i++)
				{
					edges[i].UpdateEdgeGUI(e, viewRect, editorSkin);
				}

				if (e.type == EventType.Layout)
				{
					if (selectedNode != null)
					{
						showProperties = true;
					}
				}
			}

			EditorUtility.SetDirty(this);
		}
		#endregion

#if UNITY_EDITOR
		#region Utility Methods
		void ProcessEvents(Event e, Rect viewRect)
		{
			if (viewRect.Contains(e.mousePosition))
			{
				if (e.button == 0)
				{
					if (e.type == EventType.MouseDown)
					{
						DeselectAllNodes();
						showProperties = false;
						selectedNode = null;
						bool setNode = false;
						for (int i = nodes.Count - 1; i >= 0; i--)
						{
							if (!setNode)
							{
								if (nodes[i].nodeRect.Contains(e.mousePosition))
								{
									nodes[i].isSelected = true;
									selectedNode = nodes[i];
									setNode = true;
									break;
								}
							}
						}

						if (!setNode)
						{
							for (int i = nodes.Count - 1; i >= 0; i--)
							{
								nodes[i].isSelected = false;
								selectedNode = null;
							}
						}

						if (wantsConnection)
						{
							wantsConnection = false;
						}
					}
				}
			}

			if (e.type == EventType.MouseUp)
			{

			}
		}

		void DrawConnectionToMouse(Vector2 mousePos)
		{
			Handles.BeginGUI();
			Handles.color = Color.green;
			Handles.DrawLine(new Vector3(connectionNode.nodeRect.x + connectionNode.nodeRect.width * 0.5f,
										 connectionNode.nodeRect.y + connectionNode.nodeRect.height * 0.5f, 0f),
							 new Vector3(mousePos.x, mousePos.y, 0f));


			Handles.EndGUI();
		}

		void DeselectAllNodes()
		{
			for (int i = nodes.Count - 1; i >= 0; i--)
			{
				nodes[i].isSelected = false;
			}
		}
		#endregion

#endif

		public List<SG_Edge> GetAllNodeEdges(SG_NodeBase selectedNode)
		{
			if (edges == null) return null;
			List<SG_Edge> e = new List<SG_Edge>();

			foreach (var edge in edges)
			{

				if (edge.StartNode.name == selectedNode.name || edge.EndNode.name == selectedNode.name)
					e.Add(edge);
			}

			return e;
		}


	}
}