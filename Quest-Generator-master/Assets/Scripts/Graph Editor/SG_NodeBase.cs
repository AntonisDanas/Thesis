using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
#endif

    [Serializable]
    public class SG_NodeBase : ScriptableObject
    {
        public int Index;
        public string NodeName;
        public Rect NodeRect;
        public SG_Graph ParentGraph;
        public bool IsSelected = false;

        protected GUISkin nodeSkin;

        public virtual void InitNode(string nodeName)
        {
            NodeName = nodeName;
        }

        public virtual void UpdateNode(Event e, Rect viewRect)
        {
            ProcessEvents(e, viewRect);
        }

#if UNITY_EDITOR
        public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            ProcessEvents(e, viewRect);

            GUI.Box(NodeRect, NodeName, viewSkin.GetStyle("NodeBG"));
            EditorUtility.SetDirty(this);
        }
#endif

        private void ProcessEvents(Event e, Rect viewRect)
        {
            if (e.type == EventType.MouseDown &&
                NodeRect.Contains(e.mousePosition) &&
                !IsSelected)
            {
                IsSelected = true;
                return;
            }

            if (e.type == EventType.MouseDrag &&
                IsSelected)
            {
                NodeRect.x += e.delta.x;
                NodeRect.y += e.delta.y;
                return;
            }

            if (e.type == EventType.MouseUp &&
                IsSelected)
            {
                IsSelected = false;
                return;
            }
        }
    }
}