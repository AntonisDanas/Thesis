using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class SG_ViewBase 
{
	#region Public Variables
	public string viewTitle;
	public Rect viewRect;
	#endregion

	#region Protected Variables
	protected GUISkin viewSkin;
	#endregion

	#region Constructor
	public SG_ViewBase(string title)
	{
		viewTitle = title;
		GetEditorSkin ();
	}
	#endregion

	#if UNITY_EDITOR
	#region Main Methods
	public virtual void UpdateView(Event e, Rect editorRect, Rect precentageRect)
	{
		viewRect = new Rect (editorRect.x * precentageRect.x,
		                    editorRect.y * precentageRect.y,
		                    editorRect.width * precentageRect.width,
		                    editorRect.height * precentageRect.height);

		if(viewSkin == null)
		{
			GetEditorSkin();
			return;
		}

	}

	protected virtual void ProcessEvents(){}
	#endregion
	#endif

	#region Utility Methods
	protected void GetEditorSkin()
	{
		viewSkin = (GUISkin)Resources.Load("GUISkins/EditorSkins/NodeEditorSkin");
	}
	#endregion
}
