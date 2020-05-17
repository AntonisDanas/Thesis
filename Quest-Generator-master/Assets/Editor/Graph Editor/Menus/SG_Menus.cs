using UnityEngine;
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
    public static class SG_Menus
    {
        [MenuItem("Graph Editor/Launch Graph Editor")]
        public static void InitGraphEditor()
        {
            Debug.Log("Launching a Graph Editor");
            SG_GraphEditorWindow.InitEditorWindow();
        }
    }
}