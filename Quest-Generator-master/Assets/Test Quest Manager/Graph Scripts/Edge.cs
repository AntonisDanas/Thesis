using System;

public class Edge
{
    private string edgeIndex;
    private string relationLabel;
    private string reasonLabel;

    public Edge()
        : this("", "", "") { }

    public Edge(string edgeIndex, string relationLabel)
        : this(edgeIndex, relationLabel, "") { }

    public Edge(string edgeIndex, string relationLabel, string reasonLabel)
    {
        this.edgeIndex = edgeIndex;
        this.relationLabel = relationLabel;
        this.reasonLabel = reasonLabel;
    }

    public Edge(int srcIndex, int dstIndex, string relationLabel)
        : this (srcIndex, dstIndex, relationLabel, "") { }

    public Edge(int srcIndex, int dstIndex, string relationLabel, string reasonLabel)
    {
        this.edgeIndex = srcIndex.ToString() + "-" + dstIndex.ToString();
        this.relationLabel = relationLabel;
        this.reasonLabel = reasonLabel;
    }

    public string GetEdgeIndex()
    {
        return edgeIndex;
    }

    public void SetReasonLabel(string reasonLabel)
    {
        this.reasonLabel=reasonLabel;
    }
        
    public void SetRelationLabel(string relationLabel)
    {
        this.relationLabel=relationLabel;
    }

    public string GetReasonLabel()
    {
        return this.reasonLabel;
    }
        
    public string GetRelationLabel()
    {
        return this.relationLabel;
    }
        
    public override int GetHashCode()
    {
        if (reasonLabel == null)
        {
            return this.relationLabel == null ? 0 : this.relationLabel.GetHashCode() ^ 0;
        }
        return this.relationLabel.GetHashCode() ^ this.reasonLabel.GetHashCode();
    }
        
    public override bool Equals(object obj)
    {
        return (obj is Edge) && 
                ((Edge)obj).relationLabel == relationLabel && 
                (((Edge)obj).reasonLabel == reasonLabel || ((Edge)obj).reasonLabel == null || ((Edge)obj).reasonLabel == "");
    }
}

