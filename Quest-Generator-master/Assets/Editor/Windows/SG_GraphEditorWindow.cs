using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SG_GraphEditorWindow : EditorWindow
{
    public static SG_GraphEditorWindow curWindow;

    public SG_PropertyView propertyView;
    public SG_GraphWorkView workView;

    public SG_Graph curGraph = null;

    public float ViewPercentage = 0.75f;
    
    public static void InitEditorWindow()
    {
        curWindow = (SG_GraphEditorWindow)EditorWindow.GetWindow<SG_GraphEditorWindow>();
        curWindow.titleContent = new GUIContent("Side Quest Generator");
    }

    private void OnEnable()
    {
        CreateViews();
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {
        if (propertyView == null)
        {
            CreateViews();
            return;
        }

        

        workView.UpdateView(position, new Rect(0f, 0f, ViewPercentage, 1f), curGraph);
        propertyView.UpdateView(new Rect(position.width, position.y, position.width, position.height),
                                new Rect(ViewPercentage, 0f, 1 - ViewPercentage, 1f), curGraph);

        Repaint(); //Windows keeps repainting even when not in focus

        Event e = Event.current;

        if (e.type == EventType.Layout)
            return;

        ProcessEvents(e);

        workView.ProcessEvents(e);
        propertyView.ProcessEvents(e);
    }

    private static void CreateViews()
    {
        if (curWindow != null)
        {
            curWindow.propertyView = new SG_PropertyView();
            curWindow.workView = new SG_GraphWorkView();
        }
        else
            curWindow = (SG_GraphEditorWindow)EditorWindow.GetWindow<SG_GraphEditorWindow>();
    }

    private void ProcessEvents(Event e)
    {
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow) ViewPercentage -= 0.01f;
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow) ViewPercentage += 0.01f;
    }
}
