using UnityEngine;
using UnityEditor;
using System.Collections;

public class SG_GraphEditorWindow : EditorWindow 
{
	#region Variables
	public static SG_GraphEditorWindow curWindow;

	public SG_GraphWorkView workView;
	public SG_NodePropertyView propertyView;

	public float viewPrecentage = 0.65f;
	#endregion



	#region Main Methods
	//-------------Initialize the Editor Window--------------------\\
	public static void InitEditorWindow()
	{
		curWindow = GetWindow<SG_GraphEditorWindow> ();
		curWindow.titleContent = new GUIContent("Graph Editor");

		CreateViews ();
	}

	//-------------Built in methods--------------------------------\\
	void OnEnable()
	{
		Debug.Log ("Launching Window");
	}

	void OnDestroy()
	{
		Debug.Log ("Closing Window");

		if (curWindow.workView == null ||
			curWindow.workView.curGraph == null)
			return;

		foreach (var node in curWindow.workView.curGraph.nodes)
		{
			node.name = "n_" + node.nodeName.Replace(" ", "");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
	}

	void Update()
	{
	}

	//-------------Draw Editor GUI----------------------------------\\
	void OnGUI()
	{
		//Make sure we have views to draw
		if(workView == null || propertyView == null)
		{
			CreateViews();
			return;
		}

		//Get our Current Event
		Event editorEvents = ProcessEvents ();


		//Update our Views
		workView.UpdateView (editorEvents, position, new Rect(0f, 0f, viewPrecentage, 1f));
		propertyView.UpdateView (editorEvents, new Rect(position.width, position.y, position.width, position.height), 
		                         new Rect(viewPrecentage, 0f, 1f-viewPrecentage, 1f));

		//Repaint the GUI
		Repaint ();
	}
	#endregion



	#region Utility Methods
	static void CreateViews ()
	{
		if(curWindow != null)
		{
			curWindow.workView = new SG_GraphWorkView ();
			curWindow.propertyView = new SG_NodePropertyView ();
		}
		else
		{
			curWindow = GetWindow<SG_GraphEditorWindow>();
		}
	}

	Event ProcessEvents ()
	{
		Event e = Event.current;
		//Process mouse and keyboard events here
		if (e.isKey && e.keyCode == KeyCode.RightArrow) 
		{
			viewPrecentage += 0.005f;
		}
		if (e.isKey && e.keyCode == KeyCode.LeftArrow) 
		{
			viewPrecentage -= 0.005f;
		}
		viewPrecentage = Mathf.Clamp (viewPrecentage, 0.15f, 0.85f);

		return e;
	}
	#endregion
}
