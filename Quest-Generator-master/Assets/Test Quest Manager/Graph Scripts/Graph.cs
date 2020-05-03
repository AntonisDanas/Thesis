using System;
using System.Collections.Generic;

public class Graph
{
    private List<Vertex> vertices;
    private Dictionary<string, List<Edge>> edges;
    private int verticesCount;
        
    public Graph()
        : this (new List<Vertex>(), new Dictionary<string, List<Edge>>()) { }
        
    public Graph(List<Vertex> vertices)
        : this(vertices, new Dictionary<string, List<Edge>>()) { }
        
    public Graph(List<Vertex> vertices, Dictionary<string, List<Edge>> edges)
    {
        this.vertices = vertices;
        this.edges = edges;
        verticesCount = this.vertices.Count;
    }

    public void AddVertex(string vertexLabel)
    {
        AddVertex(new Vertex(verticesCount + 1, vertexLabel));
    }

    public void AddVertex(string vertexLabel, Dictionary<string, object> vertexAttributes)
    {
        AddVertex(new Vertex(verticesCount + 1, vertexLabel, vertexAttributes));
    }

    public void AddVertex(string vertexLabel, Dictionary<string, object> vertexAttributes, List<string> edgeIndices)
    {
        AddVertex(new Vertex(verticesCount + 1, vertexLabel, vertexAttributes, edgeIndices));
    }

    public void AddVertex(Vertex vertex)
    {
        if (vertices.Contains(vertex)) return; // already exists

        if (vertex.GetIndex() == 0) vertex.SetIndex(verticesCount + 1); // new unindexed vertex

        vertices.Add(vertex);
        verticesCount++;
    }
        
    public Vertex GetVertex(int index)
    {
        if (index == 0 || index < 0 || index > verticesCount) return null;
            
        return vertices[index-1];
    }

    public Vertex GetVertexAtPosition(int index)
    {
        if (index < 0 || index >= verticesCount) return null;

        return vertices[index];
    }

    public Vertex GetPlayerVertex()
    {
        foreach (var item in vertices)
        {
            if (item.GetLabel() == "Player" || item.GetLabel() == "player")
                return item;
        }

        return null;
    }

    public void SetRelation(int srcIndex, int destIndex, string relationLabel)
    {
        SetRelation(GetVertex(srcIndex), GetVertex(destIndex), relationLabel, "");
    }

    public void SetRelation(int srcIndex, int destIndex, string relationLabel, string reasonLabel)
    {
        SetRelation(GetVertex(srcIndex), GetVertex(destIndex), relationLabel, reasonLabel);
    }
        
    public void SetRelation(Vertex src, Vertex dest, string relationLabel)
    {
        SetRelation(src, dest, relationLabel, "");
    }

    public void SetRelation(Vertex src, Vertex dest, string relationLabel, string reasonLabel)
    {

        int srcIndex = src.GetIndex();
        int destIndex = dest.GetIndex();
        int verticesCount = this.verticesCount;

        string edgeKey = srcIndex.ToString() + "-" + destIndex.ToString();
        List<Edge> temp;

        if (this.edges.ContainsKey(edgeKey))
        {
            temp = this.edges[edgeKey];
            if (!temp.Contains(new Edge(edgeKey, relationLabel, reasonLabel)))
            {
                temp.Add(new Edge(edgeKey, relationLabel, reasonLabel));
                this.edges[edgeKey] = temp;
            }
            else
            {
                Console.WriteLine("Same edge already exists");
            }
        }
        else
        {
            temp = new List<Edge>();
            temp.Add(new Edge(edgeKey, relationLabel, reasonLabel));
            this.edges.Add(edgeKey, temp);

            if (!src.GetEdgeIndices().Contains(edgeKey) && !dest.GetEdgeIndices().Contains(edgeKey))
            {
                this.vertices[srcIndex - 1].AddEdgeIndex(edgeKey);
                this.vertices[destIndex - 1].AddEdgeIndex(edgeKey);
            }
        }
    }

    public void DeleteVertex(int index)
    {
        if (this.vertices[index - 1] == null)
        {
            Console.WriteLine("This node is already deleted");
            return;
        }
        List<string> edgeKeys = this.vertices[index-1].GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            int destIndex = Convert.ToInt32(edgeKey.Split('-')[0]);
            int srcIndex = Convert.ToInt32(edgeKey.Split('-')[1]);
            if (destIndex == index) {
                this.vertices[srcIndex-1].GetEdgeIndices().Remove(edgeKey);
            }
            if (srcIndex == index)
            {
                this.vertices[destIndex-1].GetEdgeIndices().Remove(edgeKey);
            }
            this.edges.Remove(edgeKey);
        }
        this.vertices[index-1]=null;
    }
        
    public void DeleteRelation(Edge edge) {
        string edgeKey = edge.GetEdgeIndex();
        string relationLabel = edge.GetRelationLabel();
        string reasonLabel = edge.GetReasonLabel();

        if (reasonLabel == null || reasonLabel == "")
            DeleteRelation(edgeKey, relationLabel);
        else
            DeleteRelation(edgeKey, relationLabel, reasonLabel);
    }

    public void DeleteRelation(string edgeKey)
    {
        if (this.edges.ContainsKey(edgeKey))
        {
            this.vertices[Convert.ToInt32(edgeKey.Split('-')[0]) - 1].GetEdgeIndices().Remove(edgeKey);
            this.vertices[Convert.ToInt32(edgeKey.Split('-')[1]) - 1].GetEdgeIndices().Remove(edgeKey);
            this.edges.Remove(edgeKey);
        }
    }

    public void DeleteRelation(string edgeKey, string relationLabel)
    {
        if (this.edges.ContainsKey(edgeKey))
        {
            if (this.edges[edgeKey].Count == 1)
            {
                this.vertices[Convert.ToInt32(edgeKey.Split('-')[0]) - 1].GetEdgeIndices().Remove(edgeKey);
                this.vertices[Convert.ToInt32(edgeKey.Split('-')[1]) - 1].GetEdgeIndices().Remove(edgeKey);
                this.edges.Remove(edgeKey);
            }
            else
            {
                Edge edge = new Edge(edgeKey, relationLabel);
                List<Edge> temp = this.edges[edgeKey];
                temp.Remove(edge);
                this.edges[edgeKey] = temp;
            }
        }
    }

    public void DeleteRelation(string edgeKey, string relationLabel, string reasonLabel)
    {
        if (this.edges.ContainsKey(edgeKey))
        {
            if (this.edges[edgeKey].Count == 1)
            {
                this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1].GetEdgeIndices().Remove(edgeKey);
                this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1].GetEdgeIndices().Remove(edgeKey);
                this.edges.Remove(edgeKey);
            }
            else
            {
                Edge edge = new Edge(edgeKey, relationLabel, reasonLabel);
                List<Edge> temp = this.edges[edgeKey];
                temp.Remove(edge);
                this.edges[edgeKey] = temp;
            }
        }
    }

    public void DeleteAllRelationsOfVertex(int index)
    {
        Dictionary<string, List<Edge>> tempEdges = new Dictionary<string, List<Edge>>(edges);
        foreach (var edge in tempEdges)
        {
            if (edge.Key.Split('-')[0] == index.ToString() ||
                edge.Key.Split('-')[1] == index.ToString())
                DeleteRelation(edge.Key);
        }
    }

    public int GetVerticesCount()
    {
        return this.verticesCount;
    }

    public void UpdateGraph() {
        Vertex prev=new Vertex();
        bool updated = false;
        prev.SetIndex(1);
        int currentIndex = 0;
        List<int> tempIndexes = new List<int>();

        foreach(Vertex v in this.vertices.ToArray())
        {
            if (v == null)
            {
                this.vertices.RemoveAt(currentIndex);
                tempIndexes.Add(currentIndex);
                updated = true;
            }
            else
                currentIndex++;
        }
        if (updated == false)
        {
            Console.WriteLine("Noting to Update");
            return;
        }

        verticesCount = this.vertices.Count;
        currentIndex = -1;
        List<string> tempIndices = new List<string>();
        foreach(Vertex v in this.vertices)
        {
            currentIndex++;
            if (v.GetIndex() != currentIndex + 1)
            {
                int i = -1;
                if (v.GetEdgeIndices().Count == 0)
                {
                    v.SetIndex(currentIndex + 1);

                }
                foreach (string edgeKey in v.GetEdgeIndices().ToArray())
                {
                    i++;
                    int srcIndex = Convert.ToInt32(edgeKey.Split('-')[0]);
                    int destIndex = Convert.ToInt32(edgeKey.Split('-')[1]);
                    int newSrcIndex = srcIndex - binarySearchCount(tempIndexes, srcIndex);
                    int newDestIndex = destIndex - binarySearchCount(tempIndexes, destIndex);
                    Console.WriteLine(tempIndexes[0]+", count"+ binarySearchCount(tempIndexes, 2)+" ,V: " + v.GetLabel() + ", src:" + srcIndex + " ,dest:" + destIndex+", nsI: " +newSrcIndex+", ndI:"+newDestIndex);
                    if ( srcIndex == v.GetIndex())
                    {
                        string updatedEdgeKey = (currentIndex + 1) + "-" + destIndex;
                        v.GetEdgeIndices()[i] = updatedEdgeKey;
                        this.vertices[newDestIndex-1].GetEdgeIndices().Remove(edgeKey);
                        this.vertices[newDestIndex-1].GetEdgeIndices().Add(updatedEdgeKey);
                        this.edges.Add(updatedEdgeKey, this.edges[edgeKey]);
                        this.edges.Remove(edgeKey);

                    }
                    else if (destIndex == v.GetIndex())
                    {
                            string updatedEdgeKey = srcIndex + "-" + (currentIndex + 1);
                            v.GetEdgeIndices()[i] = updatedEdgeKey;
                            this.vertices[newSrcIndex-1].GetEdgeIndices().Remove(edgeKey);
                            this.vertices[newSrcIndex-1].GetEdgeIndices().Add(updatedEdgeKey);
                            this.edges.Add(updatedEdgeKey, this.edges[edgeKey]);
                            this.edges.Remove(edgeKey);
                    }
                }
                v.SetIndex(currentIndex + 1);

            }
                
        }
    }
        
    public List<Vertex> GetGraphVertices()
    {
        return this.vertices;
    }
        
    public List<Vertex> GetIncomingVertices(Vertex vertex)
    {
        List<Vertex> srcVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if(!edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
                srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
        }
        return srcVertices;
    }
        
    public List<Vertex> GetOutgoingVertices(Vertex vertex)
    {
        List<Vertex> destVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
                destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
        }
        return destVertices;
    }
        
    public List<Vertex> GetAllConnectedVertices(Vertex vertex)
    {
        List<Vertex> allVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
                allVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
            else
                allVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
        }
        return allVertices;
    }
        
    public List<Vertex> GetIncomingVerticesByRelationLabel(Vertex vertex, string relationLabel)
    {
        List<Vertex> srcVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
            {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel))
                    {
                        srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
                        break;
                    }
                }

            }

        }
        return srcVertices;
    }
        
    public List<Vertex> GetOutgoingVerticesByRelationLabel(Vertex vertex, string relationLabel)
    {
        List<Vertex> destVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString())) {
                List<Edge> edges = this.edges[edgeKey];
                    
                foreach(Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel)) {
                        destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
                        break;
                    }
                }
                    
            }
                    
        }
        return destVertices;
    }
    
    public List<Vertex> GetOutgoingVerticesByRelationLabels(Vertex vertex, List<string> relationLabels, Condition condition)
    {
        Dictionary<string, List<Vertex>> vertices = new Dictionary<string, List<Vertex>>();
        foreach (var label in relationLabels)
        {
            List<Vertex> temp = GetOutgoingVerticesByRelationLabel(vertex, label);

            if (temp == null || temp.Count == 0)
                continue;

            vertices.Add(label, temp);
        }

        List<Vertex> result = new List<Vertex>();

        switch (condition)
        {
            case Condition.AND:
                result = GetDistinctVertices(vertices);
                break;
            case Condition.OR:
            default:
                result = UniteAllVertices(vertices);
                break;
        }

        return result;
    }

    public List<Vertex> GetIncomingVerticesByRelationLabels(Vertex vertex, List<string> relationLabels, Condition condition)
    {
        Dictionary<string, List<Vertex>> vertices = new Dictionary<string, List<Vertex>>();
        foreach (var label in relationLabels)
        {
            List<Vertex> temp = GetIncomingVerticesByRelationLabel(vertex, label);

            if (temp == null || temp.Count == 0)
                continue;

            vertices.Add(label, temp);
        }

        List<Vertex> result = new List<Vertex>();

        switch (condition)
        {
            case Condition.AND:
                result = GetDistinctVertices(vertices);
                break;
            case Condition.OR:
            default:
                result = UniteAllVertices(vertices);
                break;
        }

        return result;
    }

    public List<Vertex> GetAllVerticesByRelationLabel(Vertex vertex, string relationLabel)
    {
        List<Vertex> vertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel))
                    {
                        vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
                        break;
                    }
                }

            }
            else {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel))
                    {
                        vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
                        break;
                    }
                }
            }

        }
        return vertices;
    }
        
    public List<Vertex> GetIncomingVerticesByRelationReason(Vertex vertex, string relationLabel, string reasonLabel)
    {
        List<Vertex> srcVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
            {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                    {
                        srcVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
                        break;
                    }
                }

            }

        }
        return srcVertices;
    }
        
    public List<Vertex> GetOutgoingVerticesByRelationReason(Vertex vertex, string relationLabel, string reasonLabel)
    {
        List<Vertex> destVertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                if (!this.edges.ContainsKey(edgeKey))
                    continue;

                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                    {
                        destVertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
                        break;
                    }
                }

            }

        }
        return destVertices;
    }
        
    public List<Vertex> GetAllVerticesByRelationReason(Vertex vertex, string relationLabel, string reasonLabel)
    {
        List<Vertex> vertices = new List<Vertex>();
        List<string> edgeKeys = vertex.GetEdgeIndices();
        foreach (string edgeKey in edgeKeys)
        {
            if (!edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                    {
                        vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[1])-1]);
                        break;
                    }
                }

            }
            else
            {
                List<Edge> edges = this.edges[edgeKey];

                foreach (Edge edge in edges)
                {
                    if (edge.GetRelationLabel().Equals(relationLabel) && edge.GetReasonLabel().Equals(reasonLabel))
                    {
                        vertices.Add(this.vertices[Convert.ToInt32(edgeKey.Split('-')[0])-1]);
                        break;
                    }
                }
            }

        }
        return vertices;
    }
        
    public List<string> GetOutgoingEdgesIndexes(Vertex vertex)
    {
        List<string> edges = new List<string>(); 
        foreach (string edge in vertex.GetEdgeIndices()) {
            if (edge.Split('-')[0].Equals(vertex.GetIndex().ToString()))
            {
                edges.Add(edge);
            }
        }
        return edges;
    }
        
    public List<string> GetIncomingEdgesIndexes(Vertex vertex)
    {
        List<string> edges = new List<string>();
        foreach (string edge in vertex.GetEdgeIndices())
        {
            if (edge.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                edges.Add(edge);
            }
        }
        return edges;
    }
        
    public List<string> GetAllEdgesIndexes(Vertex vertex)
    {
        return vertex.GetEdgeIndices();
    }
        
    public List<Edge> GetOutgoingEdges(Vertex vertex)
    {
        List<Edge> edges = new List<Edge>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
            {
                if (!this.edges.ContainsKey(edgeKey))
                    continue;

                edges.AddRange(this.edges[edgeKey]);
            }
        }
        return edges;
    }
        
    public List<Edge> GetIncomingEdges(Vertex vertex)
    {
        List<Edge> edges = new List<Edge>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                if (!this.edges.ContainsKey(edgeKey))
                    continue;

                edges.AddRange(this.edges[edgeKey]);
            }
        }
        return edges;
    }
        
    public List<Edge> GetAllEdges(Vertex vertex)
    {
        List<Edge> edges = new List<Edge>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (!this.edges.ContainsKey(edgeKey))
                continue;

            edges.AddRange(this.edges[edgeKey]);               
        }
        return edges;
    }
        
    public Dictionary<string, List<Edge>> GetOutgoingEdgesWithIndexes(Vertex vertex)
    {
        Dictionary<string, List<Edge>> edges = new Dictionary<string, List<Edge>>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (edgeKey.Split('-')[0].Equals(vertex.GetIndex().ToString()))
            {
                if (!this.edges.ContainsKey(edgeKey))
                    continue;

                edges.Add(edgeKey, this.edges[edgeKey]);
            }
        }
        return edges;
    }
       
    public Dictionary<string, List<Edge>> GetIncomingEdgesWithIndexes(Vertex vertex)
    {

        Dictionary<string, List<Edge>> edges = new Dictionary<string, List<Edge>>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (edgeKey.Split('-')[1].Equals(vertex.GetIndex().ToString()))
            {
                if (!this.edges.ContainsKey(edgeKey))
                    continue;

                edges.Add(edgeKey, this.edges[edgeKey]);
            }
        }
        return edges;
    }
        
    public Dictionary<string, List<Edge>> GetAllEdgesWithIndexes(Vertex vertex)
    {
        Dictionary<string, List<Edge>> edges = new Dictionary<string, List<Edge>>();
        foreach (string edgeKey in vertex.GetEdgeIndices())
        {
            if (!this.edges.ContainsKey(edgeKey))
                continue;

            edges.Add(edgeKey, this.edges[edgeKey]);
        }
        return edges;
    }

    private int binarySearchCount(List<int> list, int element)
    {
        int first = 0;
        int last = list.Count - 1;
        int mid = 0 + (list.Count - 1) / 2;
        int lesserCount = 0;
        while (first <= last)
        {
            if (list[mid] < element)
            {
                first = mid + 1;
                lesserCount += (mid - first);
            }
            else if (list[mid] == element)
            {
                return lesserCount;
            }
            else
            {
                last = mid - 1;
            }
            mid = (first + last) / 2;

        }
        lesserCount = first;
        return lesserCount;

    }

    private List<Vertex> GetDistinctVertices(Dictionary<string, List<Vertex>> vertices)
    {
        List<Vertex> final = new List<Vertex>();

        foreach (var entry in vertices)
        {

            if (final.Count == 0)
            {
                foreach (var vertex in entry.Value)
                    final.Add(vertex);

                continue;
            }

            List<Vertex> temp = new List<Vertex>();

            foreach (var vertex in entry.Value)
            {
                if (final.Contains(vertex))
                    temp.Add(vertex);
            }

            if (temp.Count == 0) // no common vertices in dictionary
                return null;

            final = temp;
        }

        return final;
    }

    private List<Vertex> UniteAllVertices(Dictionary<string, List<Vertex>> vertices)
    {
        List<Vertex> final = new List<Vertex>();

        foreach (var entry in vertices)
        {
            foreach (var vertex in entry.Value)
            {
                if (final.Contains(vertex))
                    continue;

                final.Add(vertex);
            }
        }

        return final;
    }

}

public enum Condition
{
    AND,
    OR
}
