using SideQuestGenerator.GraphEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SG_SpaceNodePopupWindow : EditorWindow
{
	#region Variables
	static SG_SpaceNodePopupWindow curPopup;
	static Vector2 mousePos = Vector2.zero;
	static SG_Graph curGraph = null;

	string nodeName = "Enter a name";
	private int typeDropdownSelection;
	private string[] typeDropdownOptions = Enum.GetValues(typeof(NodeType))
												.Cast<NodeType>()
												.Select(x => x.ToString())
												.ToArray();
	private List<string> attributeKey;
	private List<string> attributeValue;
	private List<int> attributeDropdownSelection;
	private string[] attributeDropdownOptions = new string[]
	{
		"string", "int", "boolean",
	};

	private Dictionary<string, object> attributes;
	#endregion

	#region Main Methods
	public static void InitSpaceNodePopup(int GUIID, SG_Graph cg, Vector2 mp)
	{
		curPopup = GetWindowWithRect<SG_SpaceNodePopupWindow>(new Rect(0, 0, 600, 400));
		curPopup.titleContent = new GUIContent("Space Node Popup");
		curGraph = cg;
		mousePos = mp;
	}

	private void OnEnable()
	{
		attributeKey = new List<string>();
		attributeKey.Add("");

		attributeValue = new List<string>();
		attributeValue.Add("");

		attributeDropdownSelection = new List<int>();
		attributeDropdownSelection.Add(0);

		attributes = new Dictionary<string, object>();
		attributes.Add("", "");
	}

	void OnGUI()
	{
		GUIStyle myStyle = new GUIStyle(GUI.skin.button);
		myStyle.padding = new RectOffset(0, 0, 0, 0);
		//myStyle.stretchWidth = false;


		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);

		GUILayout.BeginVertical();

		EditorGUILayout.LabelField("Create New Node:", EditorStyles.boldLabel);

		GUILayout.Space(10);
		nodeName = EditorGUILayout.TextField("Enter Name: ", nodeName);

		GUILayout.BeginHorizontal();
		typeDropdownSelection = EditorGUILayout.Popup("Node Label/Type:", typeDropdownSelection, typeDropdownOptions, myStyle);

		GUILayout.EndHorizontal();

		GUILayout.Space(15);

		EditorGUILayout.LabelField("Attributes:", EditorStyles.boldLabel);

		EditorGUIUtility.labelWidth = 90;
		EditorGUIUtility.fieldWidth = 90;

		// First attribute is the name
		GUILayout.BeginHorizontal();

		attributeDropdownSelection[0] = 0; // string type
		EditorGUILayout.LabelField("Attr. Type: ", "string", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
		attributeKey[0] = "Name";
		EditorGUILayout.LabelField("Attr. Name: ", attributeKey[0], EditorStyles.boldLabel, GUILayout.ExpandWidth(false));

		if (nodeName == "Enter a name")
			EditorGUILayout.LabelField("Attr. Value: ");
		else
		{
			attributeValue[0] = nodeName;
			EditorGUILayout.LabelField("Attr. Value: ", attributeValue[0], EditorStyles.boldLabel);
		}


		GUILayout.EndHorizontal();

		// The rest of the attributes
		for (int i = 1; i < attributeKey.Count; i++)
		{
			GUILayout.BeginHorizontal();

			attributeDropdownSelection[i] = EditorGUILayout.Popup("Attr. Type:", attributeDropdownSelection[i], attributeDropdownOptions, myStyle, GUILayout.ExpandWidth(false));
			attributeKey[i] = EditorGUILayout.TextField("Attr. Name: ", attributeKey[i], GUILayout.ExpandWidth(false));

			if (attributeDropdownSelection[i] == 0)
				attributeValue[i] = EditorGUILayout.TextField("Attr. Value: ", attributeValue[i]);
			else if (attributeDropdownSelection[i] == 1)
			{
				if (!int.TryParse(attributeValue[i], out int itemp))
					itemp = 0;

				attributeValue[i] = EditorGUILayout.IntField("Attr. Value: ", itemp).ToString();
			}
			else if (attributeDropdownSelection[i] == 2)
			{
				if (!bool.TryParse(attributeValue[i], out bool btemp))
					btemp = false;

				attributeValue[i] = EditorGUILayout.Toggle("Attr. Value: ", btemp).ToString();
			}

			GUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Add Attribute", GUILayout.Height(30)))
		{
			AddAttributeButton();
		}

		if (attributeKey == null || attributeKey.Count > 1)
		{
			if (GUILayout.Button("Remove Attribute", GUILayout.Height(30)))
			{
				RemoveAttributeButton();
			}
		}

		EditorGUI.indentLevel -= 1;

		GUILayout.Space(15);

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Create Node", GUILayout.Height(30)))
		{
			if (!CheckIfNodeNameIsValid(nodeName))
			{
				EditorUtility.DisplayDialog("Node Message:", "Please enter a valid node name", "OK");
			}
			else if (!CheckIfAttributesAreValid())
			{
				EditorUtility.DisplayDialog("Node Message:", "Please enter valid attributes", "OK");
			}
			else
			{
				attributes = ConvertAttributes();
				CreateNode();
				ClosePopup();
			}
		}

		if (GUILayout.Button("Cancel", GUILayout.Height(30)))
		{
			ClosePopup();
		}

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		GUILayout.Space(10);
	}
	#endregion

	#region Utility Methods
	void ClosePopup()
	{
		curGraph = null;

		if (curPopup == null)
		{
			curPopup = GetWindow<SG_SpaceNodePopupWindow>();
			curPopup.Close();
		}
		else
		{
			curPopup.Close();
		}
	}

	void CreateNode()
	{
		if (curGraph == null)
			return;

		Enum.TryParse(typeDropdownOptions[typeDropdownSelection], out NodeType type);

		SG_GraphUtils.CreateNode(curGraph, type, mousePos, nodeName, ConvertAttributes());
	}

	private void RemoveAttributeButton()
	{
		attributeKey.RemoveAt(attributeKey.Count - 1);
		attributeValue.RemoveAt(attributeValue.Count - 1);
		attributeDropdownSelection.RemoveAt(attributeDropdownSelection.Count - 1);
	}

	private StringObjectDictionary ConvertAttributes()
	{
		StringObjectDictionary result = new StringObjectDictionary();

		for (int i = 0; i < attributeKey.Count; i++)
		{
			switch (attributeDropdownSelection[i])
			{
				case 0:
					result.Add(attributeKey[i], attributeValue[i]);
					break;
				case 1:
					int.TryParse(attributeValue[i], out int iresult);
					result.Add(attributeKey[i], iresult);
					break;
				case 2:
					bool.TryParse(attributeValue[i], out bool bresult);
					result.Add(attributeKey[i], bresult);
					break;
				default:
					break;
			}
		}

		return result;
	}

	private bool CheckIfNodeNameIsValid(string nodeName)
	{
		if (string.IsNullOrEmpty(nodeName)) return false;
		if (string.IsNullOrWhiteSpace(nodeName)) return false;
		if (nodeName == "Enter a name...") return false;

		return true;
	}

	private bool CheckIfAttributesAreValid()
	{
		foreach (var attrKey in attributeKey)
		{
			if (string.IsNullOrEmpty(attrKey) ||
				string.IsNullOrWhiteSpace(attrKey))
				return false;
		}

		foreach (var attrValue in attributeValue)
		{
			if (string.IsNullOrEmpty(attrValue) ||
				string.IsNullOrWhiteSpace(attrValue))
				return false;
		}

		int counter = 0;
		foreach (var attrType in attributeDropdownSelection)
		{
			switch (attrType)
			{
				case 0:
					break;
				case 1:
					if (!int.TryParse(attributeValue[counter], out int iresult))
						return false;
					break;
				case 2:
					if (!bool.TryParse(attributeValue[counter], out bool bresult))
						return false;
					break;
				default:
					break;
			}
			counter++;
		}

		return true;
	}

	private void AddAttributeButton()
	{
		attributeKey.Add("");
		attributeValue.Add("");
		attributeDropdownSelection.Add(0);
	}

	#endregion
}
