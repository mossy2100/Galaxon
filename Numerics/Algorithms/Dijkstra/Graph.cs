namespace Galaxon.Numerics.Algorithms.Dijkstra;

public class Graph
{
    /// <summary>
    /// The graph's nodes.
    /// </summary>
    public List<Node> nodes = [];

    /// <summary>
    /// The graph's edges.
    /// </summary>
    public List<Edge> edges = [];

    /// <summary>
    /// Get the node with a given label.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    public Node? GetNode(string label)
    {
        return nodes.FirstOrDefault(node => node.label == label);
    }

    /// <summary>
    /// Add a new node to the graph.
    /// </summary>
    /// <param name="label"></param>
    public void AddNode(string label)
    {
        // Guard.
        if (GetNode(label) != null)
        {
            throw new ArgumentException("The graph already contains a node with this label.");
        }

        // Create and add the new node.
        nodes.Add(new Node(label));
    }

    /// <summary>
    /// Adds a new edge to the graph.
    /// </summary>
    /// <param name="node1">The label of the first node of the edge.</param>
    /// <param name="node2">The label of the second node of the edge.</param>
    /// <param name="distance">The distance between the two nodes.</param>
    /// <param name="bidirectional">A boolean indicating whether the edge is bidirectional. Default is false.</param>
    /// <exception cref="ArgumentException">Thrown when a node with the provided label is not found in the graph.</exception>
    public void AddEdge(string node1, string node2, double distance, bool bidirectional = false)
    {
        // Check if the nodes exist.
        Node? n1 = GetNode(node1);
        if (n1 == null)
        {
            throw new ArgumentException($"Node {node1} not found.");
        }
        Node? n2 = GetNode(node2);
        if (n2 == null)
        {
            throw new ArgumentException($"Node {node2} not found.");
        }

        // Add the edge.
        edges.Add(new Edge(n1, n2, distance, bidirectional));
    }

    /// <summary>
    /// Finds the shortest path from the start node to the end node using Dijkstra's algorithm.
    /// If the end node is null, it finds the shortest path to all nodes from the start node.
    /// </summary>
    /// <param name="startNodeLabel">The label of the start node.</param>
    /// <param name="endNodeLabel">The label of the end node. If null, the method will find the shortest path to all nodes.</param>
    /// <exception cref="ArgumentException">Thrown when the start node or end node is not found in the graph.</exception>
    public void ShortestPath(string startNodeLabel, string? endNodeLabel = null)
    {
        // Get the start and end nodes.
        Node startNode = GetNode(startNodeLabel) ?? throw new ArgumentException("Start node not found.");
        Node? endNode = endNodeLabel == null ? null : (GetNode(endNodeLabel) ?? throw new ArgumentException("End node not found."));

        // If the start node distance hasn't been set, set it to 0.
        if (double.IsPositiveInfinity(startNode.distanceFromStart))
        {
            startNode.distanceFromStart = 0;
        }

        while (true)
        {
            // Select the unvisited node with the shortest distance to start.
            Node? currentNode = nodes
                .Where(node => !node.visited && node.distanceFromStart < double.PositiveInfinity)
                .MinBy(n => n.distanceFromStart);

            // If we couldn't find a node, or we found the end node, we're done.
            if (currentNode == null || currentNode == endNode)
            {
                break;
            }

            // Get the edges leading from the current node.
            IEnumerable<Edge> edgesToNeighbour = edges
                .Where(e => e.node1 == currentNode || (e.bidirectional && e.node2 == currentNode));

            // Traverse the edges.
            foreach (Edge edge in edgesToNeighbour)
            {
                // Get the neighbouring node.
                Node neighbourNode = edge.node1 == currentNode ? edge.node2 : edge.node1;

                // Calculate the distance to the neighbour node through the current node.
                double tentativeDistance = currentNode.distanceFromStart + edge.distance;

                // If we found a shorter distance from the start to the neighbouring node, update it.
                if (tentativeDistance < neighbourNode.distanceFromStart)
                {
                    neighbourNode.distanceFromStart = tentativeDistance;
                }
            }

            // Mark the current node as visited.
            currentNode.visited = true;
        }
    }
}
