using UnityEngine;
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
    public class SG_EdgePopupWindow : EditorWindow
    {
        private static SG_EdgePopupWindow curPopup;
        private static SG_Graph currentGraph;
        private static SG_NodeBase sNode;
        private static SG_NodeBase eNode;

        private string label;

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
            EditorGUILayout.LabelField(sNode.NodeName);

            EditorGUILayout.LabelField("To: ", EditorStyles.boldLabel, GUILayout.MaxWidth(23f));
            EditorGUILayout.LabelField(eNode.NodeName);

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUIUtility.labelWidth = 60f;
            EditorGUIUtility.fieldWidth = 10f;
            label = EditorGUILayout.TextField("Label: ", label);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Relationship", GUILayout.Height(30)))
            {
                if (!string.IsNullOrEmpty(label))
                {
                    SG_GraphUtil.CreateEdge(currentGraph, sNode, eNode, label);
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
}