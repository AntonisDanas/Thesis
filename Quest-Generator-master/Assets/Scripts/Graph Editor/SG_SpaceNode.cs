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
    public class SG_SpaceNode : SG_NodeBase
    {
        public string Label;
        [SerializeField] public Dictionary<string, object> Attributes;

        public override void InitNode(string nodeName)
        {
            base.InitNode(nodeName);
            NodeRect = new Rect(10f, 10f, 100f, 35f);
            Label = "";
            Attributes = new Dictionary<string, object>();
        }

        public override void UpdateNode(Event e, Rect viewRect)
        {
            base.UpdateNode(e, viewRect);
        }

#if UNITY_EDITOR
        public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin viewSkin)
        {
            base.UpdateNodeGUI(e, viewRect, viewSkin);

        }
#endif
    }
}