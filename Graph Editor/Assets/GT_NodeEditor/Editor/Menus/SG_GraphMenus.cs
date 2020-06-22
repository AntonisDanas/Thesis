using UnityEngine;
using UnityEditor;
using System.Collections;

public static class SG_GraphMenus
{
	[MenuItem("Side Quest Generator/Launch Graph Editor", false, 0)]
	public static void InitNodeEditor()
	{
		SG_GraphEditorWindow.InitEditorWindow ();
	}
}

