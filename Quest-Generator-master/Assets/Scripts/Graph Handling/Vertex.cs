using System;
using System.Collections.Generic;

namespace SideQuestGenerator.GraphHandling
{
    public class Vertex
    {
        private int index;
        private string label;
        private Dictionary<string, object> vertexAttributes;
        private List<string> edgeIndices;

        public Vertex()
            : this(0, "", new Dictionary<string, object>(), new List<string>()) { }

        public Vertex(int index)
            : this(index, "", new Dictionary<string, object>(), new List<string>()) { }

        public Vertex(int index, string label)
            : this(index, label, new Dictionary<string, object>(), new List<string>()) { }

        public Vertex(int index, string label, Dictionary<string, object> vertexAttributes)
            : this(index, label, vertexAttributes, new List<string>()) { }

        public Vertex(int index, string label, Dictionary<string, object> vertexAttributes, List<string> edgeIndices)
        {
            this.index = index;
            this.label = label;
            this.vertexAttributes = vertexAttributes;
            this.edgeIndices = edgeIndices;
        }

        public void SetLabel(string label)
        {
            this.label = label;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public void AddEdgeIndex(string index)
        {
            edgeIndices.Add(index);
        }

        public void SetEdgeIndices(List<string> edgeIncides)
        {
            edgeIndices = edgeIncides;
        }

        public void SetAttribute(string key, object attribute)
        {
            if (vertexAttributes == null)
            {
                vertexAttributes = new Dictionary<string, object>();
            }
            vertexAttributes.Add(key, attribute);
        }

        public void SetAttributes(Dictionary<string, object> attributes)
        {
            vertexAttributes = attributes;
        }

        public string GetLabel()
        {
            return label;
        }

        public int GetIndex()
        {
            return index;
        }

        public List<string> GetEdgeIndices()
        {
            return edgeIndices;
        }

        public Dictionary<string, object> GetAttributes()
        {
            return vertexAttributes;
        }

        public T GetAttribute<T>(string key)
        {
            return (T)vertexAttributes[key];
        }

        public object GetAttribute(string key)
        {
            return vertexAttributes[key];
        }

        public void SetVertex(string label, Dictionary<string, object> attributes, List<string> edgeIndices)
        {
            this.label = label;
            vertexAttributes = attributes;
            this.edgeIndices = edgeIndices;
        }

        public Vertex GetVertex()
        {
            return this;
        }

        public override int GetHashCode()
        {
            return index;
        }

        public override bool Equals(object obj)
        {
            return obj is Vertex && ((Vertex)obj).index == index;
        }
    }
}