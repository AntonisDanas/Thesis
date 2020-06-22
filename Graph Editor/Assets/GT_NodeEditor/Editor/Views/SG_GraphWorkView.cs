using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SG_GraphWorkView : SG_ViewBase 
{
	#region Variables
	public SG_Graph curGraph;

	Vector2 mousePos;
	int contextNodeID = 0;
	#endregion

	#region Constructor
	public SG_GraphWorkView() : base("")
	{

	}
	#endregion

	#region Main Methods
	public override void UpdateView (Event e, Rect editorRect, Rect precentageRect)
	{
		//Update our Base Derived Class
		base.UpdateView (e, editorRect, precentageRect);

		//Draw this views GUI
		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle("WorkViewBG"));

		//Draw the Grid
		SG_GraphUtils.DrawEditorGrid (viewRect, 40f, 0.05f, Color.white);
		SG_GraphUtils.DrawEditorGrid (viewRect, 120f, 0.15f, Color.white);

		//Draw our nodes
		if(curGraph != null)
		{
			viewTitle = curGraph.graphName;
			curGraph.UpdateGraph(viewRect, e, viewSkin);
		}
		else
		{
			viewTitle = "No Graph";
		}

		//Process this views Events
		ProcessEvents (e);
	}
	#endregion

	#region Utility Methods
	protected virtual void ProcessEvents(Event e)
	{
		if(viewRect.Contains(e.mousePosition))
		{
			if (e.type == EventType.ContextClick)
			{
				contextNodeID = 0;
				mousePos = e.mousePosition;
				bool inNode = false;
				if(curGraph != null)
				{
					if(curGraph.nodes.Count > 0)
					{
						for(int i = 0; i < curGraph.nodes.Count; i++)
						{
							if(curGraph.nodes[i].nodeRect.Contains(e.mousePosition))
							{
								inNode = true;
								contextNodeID = i;
								break;
							}
						}
					}
				}

				if(!inNode)
				{
					ProcessContextMenu(e, 0);
				}
				else
				{
					if (curGraph.wantsConnection)
					{
						if (curGraph.nodes[contextNodeID] != curGraph.connectionNode)
							SG_EdgePopupWindow.InitEdgePopup(curGraph, curGraph.connectionNode, curGraph.nodes[contextNodeID]);

						curGraph.wantsConnection = false;
					}
					else 
						ProcessContextMenu(e, 1);
				}
			}
		}
	}

	void ProcessContextMenu(Event e, int contextID)
	{
		// Now create the menu, add items and show it
		GenericMenu menu = new GenericMenu ();

		if(contextID == 0)
		{
			menu.AddItem (new GUIContent ("Create Graph"), false, ContextCallback, "Create Graph");
			menu.AddItem (new GUIContent ("Load Graph"), false, ContextCallback, "Load Graph");

			if(curGraph != null)
			{
				menu.AddSeparator ("");
				menu.AddItem (new GUIContent ("Unload Graph"), false, ContextCallback, "Unload Graph");

				menu.AddSeparator ("");
				menu.AddItem (new GUIContent ("Add Node"), false, ContextCallback, "Add Node");
			}
		}

		if(contextID == 1)
		{
			if(curGraph != null)
			{
				menu.AddItem (new GUIContent ("Delete Node"), false, ContextCallback, "Delete Node");
				menu.AddItem(new GUIContent("Create Transition"), false, ContextCallback, "Create Transition");
			}
		}
		
		menu.ShowAsContext ();
		
		e.Use();
	}

	void ContextCallback(object obj)
	{
		switch(obj.ToString())
		{
			case "Create Graph":
				SG_GraphPopupWindow.InitGraphPopup(0);
				break;

			case "Load Graph":
				curGraph = SG_GraphUtils.LoadGraph();
				SetPropertyView(curGraph);
				break;

			case "Unload Graph":
				curGraph = null;
				SetPropertyView(curGraph);
				break;

			case "Add Node":
				SG_SpaceNodePopupWindow.InitSpaceNodePopup(0, curGraph, mousePos);
				break;

			case "Delete Node":
				SG_GraphUtils.DeleteNode(contextNodeID, curGraph);
				break;

			case "Create Transition":
				curGraph.wantsConnection = true;
				curGraph.connectionNode = curGraph.nodes[contextNodeID];
				break;

			default:
				break;
		}
	}

	public void SetPropertyView(SG_Graph curGraph)
	{
		SG_GraphEditorWindow curEditor = (SG_GraphEditorWindow)EditorWindow.GetWindow<SG_GraphEditorWindow>();
		if(curEditor != null)
		{
			if(curEditor.propertyView != null)
			{
				curEditor.propertyView.curGraph = curGraph;
			}
		}
	}
	#endregion
}
