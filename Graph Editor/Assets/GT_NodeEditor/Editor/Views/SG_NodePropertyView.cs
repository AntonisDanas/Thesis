using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SG_NodePropertyView : SG_ViewBase 
{
	#region Variables
	public SG_Graph curGraph;
	#endregion

	#region Constructor
	public SG_NodePropertyView() : base("Property View")
	{
		
	}
	#endregion
	
	#region Main Methods
	public override void UpdateView (Event e, Rect editorRect, Rect precentageRect)
	{
		base.UpdateView (e, editorRect, precentageRect);
		GUI.Box (viewRect, viewTitle, viewSkin.GetStyle("PropertyViewBG"));

		

		//Draw our nodes
		if (curGraph != null)
		{
			viewTitle = curGraph.graphName;

			GUILayout.BeginArea(viewRect);
			GUILayout.BeginHorizontal();

			GUILayout.Space(40);

			GUILayout.BeginVertical();
			GUILayout.Space(60);

			//EditorGUIUtility.labelWidth = 70;

			if (!curGraph.showProperties)
			{
				EditorGUILayout.LabelField("Type: NONE", SG_EditorStyleUtils.LabelPlainWhite(200));
			}
			else
			{
				curGraph.selectedNode.NodeEditorGUI();
			}

			GUILayout.EndVertical();

			GUILayout.Space(40);

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		else
		{
			viewTitle = "No Graph";
		}

		
	}
	#endregion

	#region Utility Methods  
	#endregion
}
