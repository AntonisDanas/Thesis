using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SG_GraphPopupWindow : EditorWindow
{
    private static SG_GraphPopupWindow curPopup;
    private string graphName = "Enter a name...";

    public static void InitGraphPopup()
    {
        curPopup = EditorWindow.GetWindow<SG_GraphPopupWindow>();
        curPopup.titleContent = new GUIContent("Graph Popup");
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Create New Graph:", EditorStyles.boldLabel);
        graphName = EditorGUILayout.TextField("Enter Name: ", graphName);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Graph", GUILayout.Height(30)))
        {
            if (!string.IsNullOrEmpty(graphName) && graphName != "Enter a name...")
            {
                SG_GraphUtil.CreateNewGraph(graphName);
                curPopup.Close();
            }
            else
            {
                EditorUtility.DisplayDialog("Graph Message:", "Please enter a valid graph name!", "OK");
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
}