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
	public class SG_NodeBase : ScriptableObject
	{
		#region Public Variables
		public bool isSelected = false;
		public Rect nodeRect = new Rect(10f, 10f, 180f, 45f);
		public string nodeName;
		public NodeType nodeType;
		public string nodePath;
		public SG_Graph parentGraph;
		public int Index;
		public string Label;
		public StringObjectDictionary Attributes;
		#endregion

		#region Private Variables
		GUIStyle selectedStyle;
		#endregion

		#region SubClasses
		[Serializable]
		public class GT_NodeInput
		{
			public bool isOccuppied = false;
			public SG_NodeBase inputNode;
		}

		[Serializable]
		public class GT_NodeOutput
		{
			public bool isOccupied = false;
		}
		#endregion

		#region Main Methods
		public virtual void InitNode()
		{

		}

		public virtual void UpdateNode(Rect viewRect, Event e, GUISkin editorSkin)
		{
			if (!isSelected)
			{
				GUI.Box(nodeRect, nodeType.ToString(), editorSkin.GetStyle("NodeBG"));
			}
			else
			{
				if (selectedStyle == null)
				{
					selectedStyle = new GUIStyle();
				}

				selectedStyle.fontSize = editorSkin.GetStyle("NodeBG").fontSize;
				selectedStyle.alignment = editorSkin.GetStyle("NodeBG").alignment;
				selectedStyle.border = editorSkin.GetStyle("NodeBG").border;
				selectedStyle.padding = editorSkin.GetStyle("NodeBG").padding;
				selectedStyle.normal.background = editorSkin.GetStyle("NodeBG").hover.background;
				selectedStyle.normal.textColor = editorSkin.GetStyle("NodeBG").hover.textColor;
				selectedStyle.hover.background = editorSkin.GetStyle("NodeBG").hover.background;
				selectedStyle.hover.textColor = editorSkin.GetStyle("NodeBG").hover.textColor;
				GUI.Box(nodeRect, nodeType.ToString(), selectedStyle);
			}

			ProcessEvents(e, viewRect);
			nodeRect.x = Mathf.Clamp(nodeRect.x, viewRect.x, viewRect.width - nodeRect.width);
			nodeRect.y = Mathf.Clamp(nodeRect.y, viewRect.y, viewRect.height - nodeRect.height);
			EditorUtility.SetDirty(this);
		}

#if UNITY_EDITOR
		public virtual void NodeEditorGUI()
		{
			EditorGUILayout.LabelField("Type: " + nodeType.ToString(), SG_EditorStyleUtils.LabelPlainWhite(200));
		}
#endif
		#endregion

		#region Utility Methods
		void ProcessEvents(Event e, Rect viewRect)
		{
			if (viewRect.Contains(e.mousePosition))
			{
				if (e.button == 0)
				{
					if (e.type == EventType.MouseDown)
					{

					}

					if (e.type == EventType.MouseDrag && isSelected)
					{
						nodeRect.x += e.delta.x;
						nodeRect.y += e.delta.y;
					}

					if (e.type == EventType.MouseUp)
					{

					}
				}
			}
		}

		protected virtual void DrawInputLines() { }
		#endregion
	}
}