using UnityEngine;
using UnityEditor;

public class SG_EdgePopupWindow : EditorWindow
{
    private static SG_EdgePopupWindow curPopup;
    private static SG_Graph currentGraph;
    private static SG_NodeBase sNode;
    private static SG_NodeBase eNode;

    private string label;
    private string reason = "";

    public static void InitEdgePopup(SG_Graph curGraph, SG_NodeBase startNode, SG_NodeBase endNode)
    {
        curPopup = GetWindow<SG_EdgePopupWindow>();
        curPopup.titleContent = new GUIContent("Edge Popup");
        currentGraph = curGraph;
        sNode = startNode;
        eNode = endNode;
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        EditorGUIUtility.labelWidth = 10f;
        EditorGUILayout.LabelField("From: ", EditorStyles.boldLabel, GUILayout.MaxWidth(40f));
        EditorGUILayout.LabelField(sNode.nodeName);

        EditorGUILayout.LabelField("To: ", EditorStyles.boldLabel, GUILayout.MaxWidth(23f));
        EditorGUILayout.LabelField(eNode.nodeName);

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUIUtility.labelWidth = 60f;
        EditorGUIUtility.fieldWidth = 10f;
        label = EditorGUILayout.TextField("Label: ", label);
        reason = EditorGUILayout.TextField("Reason: ", reason);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Relationship", GUILayout.Height(30)))
        {
            if (!string.IsNullOrEmpty(label))
            {
                SG_GraphUtils.CreateEdge(currentGraph, sNode, eNode, label, reason);
                curPopup.Close();
            }
            else
            {
                EditorUtility.DisplayDialog("Edge Message:", "Please enter a valid relationship label!", "OK");
            }
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(30)))
        {
            curPopup.Close();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(20);
    }
}
