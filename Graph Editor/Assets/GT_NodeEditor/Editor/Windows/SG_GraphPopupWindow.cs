using UnityEngine;
using UnityEditor;
using System.Collections;

public class SG_GraphPopupWindow : EditorWindow 
{
	#region Variables
	static SG_GraphPopupWindow curPopup;
	string wantedName = "Enter a name";
	#endregion

	#region Main Methods
	public static void InitGraphPopup(int GUIID)
	{
		curPopup = (SG_GraphPopupWindow)EditorWindow.GetWindow<SG_GraphPopupWindow> ();
		curPopup.titleContent = new GUIContent("Graph Popup");
	}

	void OnGUI()
	{
		GUILayout.Space (10);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (10);

		GUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Create new Graph", EditorStyles.boldLabel);

		wantedName = EditorGUILayout.TextField ("Enter Name: ", wantedName);

		GUILayout.Space (10);

		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("Create Graph", GUILayout.Height(40), GUILayout.Width((position.width-20) * 0.5f)))
		{
			if(wantedName != "Enter a name" && !string.IsNullOrEmpty(wantedName))
			{
				SG_GraphUtils.CreateNewGraph(wantedName);  
				ClosePopup();
			}
			else
			{
				EditorUtility.DisplayDialog("Node Editor Message", "Please enter a valid graph name!", "OK");
			}
		}

		if(GUILayout.Button("Cancel", GUILayout.Height(40)))
		{
			ClosePopup();
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

		GUILayout.Space (10);
		GUILayout.EndHorizontal ();  
		GUILayout.Space (10);
	}
	#endregion

	#region Utility Methods
	void ClosePopup()
	{
		if(curPopup == null)
		{
			curPopup = (SG_GraphPopupWindow)EditorWindow.GetWindow<SG_GraphPopupWindow>();
			curPopup.Close();
		}
		else
		{
			curPopup.Close();
		}
	}
	#endregion
}
