using System;
using UnityEngine;

namespace SideQuestGenerator.GraphEditor
{
    [Serializable]
    public class SG_ViewBase
    {
        public string ViewTitle;
        public Rect ViewRect;

        protected GUISkin viewSkin;
        protected SG_Graph curGraph;

        public SG_ViewBase(string title)
        {
            ViewTitle = title;
            GetEditorSkin();
        }

        public virtual void UpdateView(Rect editorRect, Rect percentageRect, SG_Graph curGraph)
        {
            if (viewSkin == null)
            {
                GetEditorSkin();
                return;
            }

            this.curGraph = curGraph;

            ViewRect = new Rect(editorRect.x * percentageRect.x,
                                editorRect.y * percentageRect.y,
                                editorRect.width * percentageRect.width,
                                editorRect.height * percentageRect.height);

            if (curGraph != null)
            {
                curGraph.UpdateGraph();
            }
        }
        public virtual void ProcessEvents(Event e)
        {
        }

        protected void GetEditorSkin()
        {
            viewSkin = (GUISkin)Resources.Load("GUISkins/EditorSkins/NodeEditorSkin");
        }
    }
}