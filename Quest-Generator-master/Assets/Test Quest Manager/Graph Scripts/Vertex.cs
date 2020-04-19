using System;
using System.Collections.Generic;

public class Vertex
{
    private int index;
    private String label;
    private Dictionary<String, object> vertexAttributes;
    private List<String> edgeIndices;
        
    public Vertex()
        : this(0, "", new Dictionary<String, object>(), new List<String>()) { }

    public Vertex(int index)
        : this(index, "", new Dictionary<String, object>(), new List<String>()) { }

    public Vertex(int index, String label)
        : this(index, label, new Dictionary<String, object>(), new List<String>()) { }

    public Vertex(int index, String label, Dictionary<String, object> vertexAttributes)
        : this(index, label, vertexAttributes, new List<String>()) { }
        
    public Vertex(int index, String label, Dictionary<String, object> vertexAttributes, List<String> edgeIndices)
    {
        this.index = index;
        this.label = label;
        this.vertexAttributes = vertexAttributes;
        this.edgeIndices = edgeIndices;
    }
      
    public void SetLabel(String label)
    {
        this.label = label;
    }
        
    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void AddEdgeIndex(String index)
    {
        this.edgeIndices.Add(index);
    }

    public void SetEdgeIndices(List<String> edgeIncides)
    {
        this.edgeIndices = edgeIncides;
    }

    public void SetAttribute(String key, object attribute)
    {
        if (this.vertexAttributes == null)
        {
            this.vertexAttributes = new Dictionary<String, object>();
        }
        this.vertexAttributes.Add(key, attribute);
    }

    public void SetAttributes(Dictionary<String, object> attributes)
    {
        this.vertexAttributes = attributes;
    }
        
    public String GetLabel()
    {
        return this.label;
    }
        
    public int GetIndex()
    {
        return this.index;
    }
        
    public List<String> GetEdgeIndices()
    {
        return this.edgeIndices;
    }
        
    public Dictionary<String, object> GetAttributes()
    {
        return this.vertexAttributes;
    }
        
    public T GetAttribute<T>(String key)
    {
        return (T)this.vertexAttributes[key];
    }
        
    public object GetAttribute(String key)
    {
        return this.vertexAttributes[key];
    }

    public void SetVertex(String label, Dictionary<String, object> attributes, List<String> edgeIndices)
    {
        this.label = label;
        this.vertexAttributes = attributes;
        this.edgeIndices = edgeIndices;
    }

    public Vertex GetVertex()
    {
        return this;
    }
        
    public override int GetHashCode()
    {
        return this.index;
    }
        
    public override bool Equals(object obj)
    {
        return (obj is Vertex) && ((Vertex)obj).index == index;
    }
}

