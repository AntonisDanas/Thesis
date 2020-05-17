using UnityEngine;
using System;
using UnityEditor;

namespace SideQuestGenerator.GraphEditor
{
    [Serializable]
    public class SG_PropertyView : SG_ViewBase
    {
        private Vector2 mousePos;
        private SG_SpaceNode selectedNode;

        public SG_PropertyView() : base("Property View") { }

        public override void UpdateView(Rect editorRect, Rect percentageRect, SG_Graph curGraph)
        {
            base.UpdateView(editorRect, percentageRect, curGraph);

            GUI.Box(ViewRect, ViewTitle, viewSkin.GetStyle("PropertyViewBG"));

            GUILayout.BeginArea(ViewRect);

            if (selectedNode != null)
            {
                Handles.BeginGUI();
                Vector2 pos = new Vector2();
                pos.x = ViewRect.x + 10;
                pos.y = ViewRect.y + 10;
                Handles.Label(pos, "Test");
                Handles.EndGUI();
            }

            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);

            mousePos = e.mousePosition;

            if (e.type == EventType.MouseDown && (e.button == 0 || e.button == 1))
            {
                selectedNode = CheckIfMouseOverNode() as SG_SpaceNode;

                if (selectedNode == null)
                    return;

                Debug.Log(selectedNode.Attributes.Count);
            }
            
        }

        private SG_NodeBase CheckIfMouseOverNode()
        {
            if (curGraph != null)
            {
                foreach (var node in curGraph.Nodes)
                {
                    if (node.NodeRect.Contains(mousePos))
                    {
                        return node;
                    }
                }
            }

            return null;
        }
    }
}