using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class SG_PropertyView : SG_ViewBase
{

    public SG_PropertyView() : base("Property View") { }

    public override void UpdateView(Rect editorRect, Rect percentageRect, SG_Graph curGraph)
    {
        base.UpdateView(editorRect, percentageRect, curGraph);

        GUI.Box(ViewRect, ViewTitle, viewSkin.GetStyle("PropertyViewBG"));

        GUILayout.BeginArea(ViewRect);
        
        GUILayout.EndArea();
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);
    }
}
