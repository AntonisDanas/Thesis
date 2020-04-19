using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SG_NodePopupWindow : EditorWindow
{
    private static SG_NodePopupWindow curPopup;
    private static SG_Graph currentGraph;
    private static Vector2 mousePosition;

    private string nodeName = "Enter a name...";
    
    private List<string> labels;
    private Dictionary<string, object> properties;

    public static void InitNodePopup(SG_Graph curGraph, Vector2 mousePos)
    {
        curPopup = EditorWindow.GetWindow<SG_NodePopupWindow>();
        curPopup.titleContent = new GUIContent("Node Popup");
        currentGraph = curGraph;
        mousePosition = mousePos;
    }

    private void OnEnable()
    {
        labels = new List<string>();
        labels.Add("");
        properties = new Dictionary<string, object>();
        properties.Add("", "");
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Create New Node:", EditorStyles.boldLabel);
        nodeName = EditorGUILayout.TextField("Enter Name: ", nodeName);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Labels:", EditorStyles.boldLabel);
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < labels.Count; i++)
        {
            
            GUILayout.BeginHorizontal();

            labels[i] = EditorGUILayout.TextField("Label " + (i + 1) + ":", labels[i]);
            //Debug.Log(labels[i]);
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
            {
                labels.Add("");
            }

            if (labels.Count > 1 && GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
            {
                labels.RemoveAt(i);
            }

            GUILayout.EndHorizontal();
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
            else if (!CheckIfLabelsAreValid())
            {
                EditorUtility.DisplayDialog("Node Message:", "Please enter valid labels", "OK");
            }
            else
            {
                SG_GraphUtil.CreateNode(currentGraph, mousePosition, nodeName, labels);
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

    private bool CheckIfLabelsAreValid()
    {
        if (!(labels.Count > 1) &&
            (string.IsNullOrEmpty(labels[0]) ||
            string.IsNullOrWhiteSpace(labels[0])))
            return false;

        foreach (string label in labels)
        {
            if (string.IsNullOrEmpty(label) ||
                string.IsNullOrWhiteSpace(label))
                return false;
        }

        return true;
    }

    private bool CheckIfNodeNameIsValid(string nodeName)
    {
        if (string.IsNullOrEmpty(nodeName)) return false;
        if (string.IsNullOrWhiteSpace(nodeName)) return false;
        if (nodeName == "Enter a name...") return false;

        return true;
    }

}
