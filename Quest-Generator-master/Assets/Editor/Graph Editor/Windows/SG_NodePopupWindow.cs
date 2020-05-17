using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SideQuestGenerator.GraphEditor
{
    public class SG_NodePopupWindow : EditorWindow
    {
        private static SG_NodePopupWindow curPopup;
        private static SG_Graph currentGraph;
        private static Vector2 mousePosition;

        private string nodeName = "Enter a name...";
        private string label = "Enter a label...";

        private List<string> attributeKey;
        private List<string> attributeValue;
        private List<int> attributeDropdownSelection;
        private string[] attributeDropdownOptions = new string[]
        {
        "string", "int", "boolean",
        };

        private Dictionary<string, object> attributes;

        public static void InitNodePopup(SG_Graph curGraph, Vector2 mousePos)
        {
            curPopup = GetWindowWithRect<SG_NodePopupWindow>(new Rect(0, 0, 600, 400));
            curPopup.titleContent = new GUIContent("Node Popup");
            currentGraph = curGraph;
            mousePosition = mousePos;
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

        private void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("Create New Node:", EditorStyles.boldLabel);

            GUILayout.Space(10);
            nodeName = EditorGUILayout.TextField("Enter Name: ", nodeName);
            label = EditorGUILayout.TextField("Enter Label: ", label);

            GUILayout.Space(15);

            EditorGUILayout.LabelField("Attributes:", EditorStyles.boldLabel);

            EditorGUI.indentLevel += 1;

            EditorGUIUtility.labelWidth = 90;
            EditorGUIUtility.fieldWidth = 90;

            for (int i = 0; i < attributeKey.Count; i++)
            {

                GUIStyle myStyle = new GUIStyle(GUI.skin.button);
                myStyle.padding = new RectOffset(0, 0, 0, 0);


                GUILayout.BeginHorizontal();

                attributeDropdownSelection[i] = EditorGUILayout.Popup("Attr. Type:", attributeDropdownSelection[i], attributeDropdownOptions, myStyle);
                attributeKey[i] = EditorGUILayout.TextField("Attr. Name: ", attributeKey[i]);
                attributeValue[i] = EditorGUILayout.TextField("Attr. Value: ", attributeValue[i]);

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
                else if (!CheckIfLabelIsValid())
                {
                    EditorUtility.DisplayDialog("Node Message:", "Please enter a valid label", "OK");
                }
                else if (!CheckIfAttributesAreValid())
                {
                    EditorUtility.DisplayDialog("Node Message:", "Please enter valid attributes", "OK");
                }
                else
                {
                    attributes = ConvertAttributes();
                    SG_GraphUtil.CreateNode(currentGraph, mousePosition, nodeName, label, attributes);
                    curPopup.Close();
                }
            }

            if (GUILayout.Button("Cancel", GUILayout.Height(30)))
            {
                curPopup.Close();
            }
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();

            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }

        private void RemoveAttributeButton()
        {
            attributeKey.RemoveAt(attributeKey.Count - 1);
            attributeValue.RemoveAt(attributeValue.Count - 1);
            attributeDropdownSelection.RemoveAt(attributeDropdownSelection.Count - 1);
        }

        private Dictionary<string, object> ConvertAttributes()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

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

        private bool CheckIfLabelIsValid()
        {
            if (string.IsNullOrEmpty(label) ||
                string.IsNullOrWhiteSpace(label))
                return false;

            return true;
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

    }
}