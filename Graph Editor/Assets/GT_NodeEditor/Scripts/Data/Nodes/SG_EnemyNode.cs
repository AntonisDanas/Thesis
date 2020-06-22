using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SG_EnemyNode : SG_NodeBase
{
	#region Public Variables
	public string Label;
	public StringObjectDictionary Attributes;
	#endregion

	#region Main Methods
	public override void InitNode()
	{
		base.InitNode();
		nodeType = NodeType.Enemy;
		Label = "Enemy";
		Attributes = new StringObjectDictionary();
	}

	public override void UpdateNode(Rect viewRect, Event e, GUISkin editorSkin)
	{
		base.UpdateNode(viewRect, e, editorSkin);

		var oldColor = EditorStyles.label.normal.textColor;

		//Draw node Properties on actual node gui
		GUILayout.BeginArea(nodeRect);
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		GUILayout.BeginVertical();
		GUILayout.Space(20);

		GUILayout.BeginHorizontal();
		EditorStyles.label.normal.textColor = Color.white;
		EditorGUILayout.LabelField("Name: ", GUILayout.Width(40));
		EditorGUILayout.LabelField(nodeName);
		GUILayout.EndHorizontal();

		GUILayout.Space(20);
		GUILayout.EndVertical();
		GUILayout.Space(20);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		EditorStyles.label.normal.textColor = oldColor;
	}

	public override void NodeEditorGUI()
	{
		base.NodeEditorGUI();
		if (parentGraph == null)
			return;

		if (!parentGraph.showProperties)
			return;

		GUILayout.Space(10);

		EditorGUILayout.BeginHorizontal();
		try
		{
			EditorGUILayout.LabelField("Name: ", SG_EditorStyleUtils.LabelPlainWhite(50));
			nodeName = EditorGUILayout.TextField(nodeName);
		}
		finally
		{
			EditorGUILayout.EndHorizontal();
		}

		GUILayout.Space(20);

		EditorGUILayout.LabelField("Attributes: ", SG_EditorStyleUtils.LabelBoldWhite(150));

		GUILayout.Space(10);

		GUILayout.BeginVertical();

		ShowNameAttribute();

		ShowRestAttributes();

		GUILayout.Space(20);

		EditorGUILayout.LabelField("Relationships: ", SG_EditorStyleUtils.LabelBoldWhite(150));

		GUILayout.Space(10);

		ShowRelationships();

		GUILayout.EndVertical();

	}

	private void ShowRelationships()
	{
		List<SG_Edge> edges = parentGraph.GetAllNodeEdges(this);
		GUIStyle button = new GUIStyle(GUI.skin.button);
		button.margin = new RectOffset(280, 0, 0, 0);

		for (int i = 0; i < edges.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();

			try
			{
				EditorGUILayout.LabelField(edges[i].ToString(), SG_EditorStyleUtils.LabelPlainWhite(280), GUILayout.ExpandWidth(false));


				if (GUILayout.Button("Delete Relationship", button, GUILayout.Height(15), GUILayout.Width(150)))
				{
					DeleteEdge(edges[i]);
				}

			}
			finally
			{
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	private void DeleteEdge(SG_Edge edge)
	{
		if (parentGraph == null) return;
		if (!parentGraph.edges.Contains(edge)) return;

		parentGraph.edges.Remove(edge);
		GameObject.DestroyImmediate(edge, true);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private void ShowRestAttributes()
	{
		if (Attributes == null || (Attributes.ContainsKey("Name") && Attributes.Count == 1))
			return;

		List<string> attrTypes = new List<string>();
		List<string> attrKeys = new List<string>();
		List<string> attrValues = new List<string>();

		foreach (var attr in Attributes)
		{
			if (attr.Key == "Name")
				continue;

			attrTypes.Add(GetAttributeType(attr.Key));
			attrKeys.Add(attr.Key);
			attrValues.Add(attr.Value.ToString());
		}


		for (int i = 0; i < attrTypes.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();

			try
			{
				EditorGUIUtility.labelWidth = 40;
				EditorGUIUtility.fieldWidth = 30;
				EditorGUILayout.LabelField("Attr. Type:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField(attrTypes[i], SG_EditorStyleUtils.LabelPlainWhite(80), GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField("Attr. Name:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField(attrKeys[i], SG_EditorStyleUtils.LabelPlainWhite(80), GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField("Attr. Value:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));

				if (attrTypes[i] == "string")
					attrValues[i] = EditorGUILayout.TextField(attrValues[i]);
				else if (attrTypes[i] == "int")
				{
					int.TryParse(attrValues[i], out int itemp);
					attrValues[i] = EditorGUILayout.IntField(itemp).ToString();
				}
				else
				{
					bool.TryParse(attrValues[i], out bool btemp);
					attrValues[i] = EditorGUILayout.Toggle(btemp).ToString();
				}
			}
			finally
			{
				EditorGUILayout.EndHorizontal();
			}
		}

		ApplyChangesToAttributes(attrTypes, attrKeys, attrValues);

	}

	private void ApplyChangesToAttributes(List<string> attrTypes, List<string> attrKeys, List<string> attrValues)
	{
		for (int i = 0; i < attrTypes.Count; i++)
		{
			switch (attrTypes[i])
			{
				case "int":
					if (!Attributes.ContainsKey(attrKeys[i]))
						continue;
					int.TryParse(attrValues[i], out int itemp);
					Attributes[attrKeys[i]] = itemp;
					break;
				case "boolean":
					if (!Attributes.ContainsKey(attrKeys[i]))
						continue;
					bool.TryParse(attrValues[i], out bool btemp);
					Attributes[attrKeys[i]] = btemp;
					break;
				default:
					if (!Attributes.ContainsKey(attrKeys[i]))
						continue;
					Attributes[attrKeys[i]] = attrValues[i].ToString();
					break;
			}
		}
	}

	private void ShowNameAttribute()
	{
		// First attribute is the name
		GUILayout.BeginHorizontal();

		try
		{
			EditorGUIUtility.labelWidth = 40;
			EditorGUIUtility.fieldWidth = 30;
			EditorGUILayout.LabelField("Attr. Type:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("string", SG_EditorStyleUtils.LabelPlainWhite(80), GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Attr. Name:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Name", SG_EditorStyleUtils.LabelPlainWhite(80), GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Attr. Value:", SG_EditorStyleUtils.LabelBoldWhite(80), GUILayout.ExpandWidth(false));

			Attributes["Name"] = nodeName;
			EditorGUILayout.LabelField(Attributes["Name"].ToString(), SG_EditorStyleUtils.LabelPlainWhite(80));

		}
		finally
		{
			EditorGUILayout.EndHorizontal();
		}
	}

	private string GetAttributeType(string key)
	{
		if (int.TryParse(Attributes[key].ToString(), out int itemp))
			return "int";
		else if (bool.TryParse(Attributes[key].ToString(), out bool btemp))
			return "boolean";
		else
			return "string";
	}

	#endregion
}

