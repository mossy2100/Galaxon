namespace Galaxon.Numerics.Algorithms.Dijkstra;

public class Node
{
    /// <summary>
    /// The node label.
    /// </summary>
    public readonly string label;

    /// <summary>
    /// The distance from the start node to this node.
    /// </summary>
    public double distanceFromStart = double.PositiveInfinity;

    /// <summary>
    /// If the node has been visited (processed).
    /// </summary>
    public bool visited = false;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="label">The node label.</param>
    public Node(string label)
    {
        this.label = label;
    }

    /// <summary>
    /// Get a string representation of the node.
    /// </summary>
    /// <returns>A string representation of the node.</returns>
    public override string ToString()
    {
        string isVisited = visited ? "visited" : "unvisited";
        return $"{label,10} {distanceFromStart,10} {isVisited,10}";
    }
}
