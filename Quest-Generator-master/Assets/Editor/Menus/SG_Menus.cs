using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SG_Menus
{
    [MenuItem("Graph Editor/Launch Graph Editor")]
    public static void InitGraphEditor()
    {
        Debug.Log("Launching a Graph Editor");
        SG_GraphEditorWindow.InitEditorWindow();
    }
}
