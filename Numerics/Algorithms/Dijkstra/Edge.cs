namespace Galaxon.Numerics.Algorithms.Dijkstra;

public class Edge
{
    /// <summary>
    /// If this is true, the edge can be traversed in both directions.
    /// If this is false, the edge can only be traversed in one direction, from node1 to node2.
    /// </summary>
    public readonly bool bidirectional;

    /// <summary>
    /// The distance between the two nodes.
    /// </summary>
    public readonly double distance;

    /// <summary>
    /// The start node.
    /// </summary>
    public readonly Node node1;

    /// <summary>
    /// The finish node.
    /// </summary>
    public readonly Node node2;

    public Edge(Node node1, Node node2, double distance, bool bidirectional = false)
    {
        this.node1 = node1;
        this.node2 = node2;
        this.distance = distance;
        this.bidirectional = bidirectional;
    }

    /// <summary>
    /// Get a string representation of the edge.
    /// </summary>
    /// <returns>A string representation of the edge.</returns>
    public override string ToString()
    {
        string arrow = bidirectional ? "<->" : "->";
        string edgeString = $"{node1.label} {arrow} {node2.label}";
        return $"{edgeString,15} {distance,10}";
    }
}
