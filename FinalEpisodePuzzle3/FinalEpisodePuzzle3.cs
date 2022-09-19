using System.Collections.ObjectModel;
using Data;

namespace FinalEpisodePuzzle3;

internal class FinalEpisodePuzzle3
{
    private static void Main(string[] args)
    {
        Console.WriteLine(new FinalEpisodePuzzle3().Solve());
    }

    private string Solve()
    {
        var cables = new MachineRoom().ReadFromFile().ToList().AsReadOnly();

        var immutableGraphTopology = new ImmutableGraphTopology(cables);
        var pathFinder = new PathFinding(immutableGraphTopology.StartNode, immutableGraphTopology.EndNode);
        var solutionSpace = new SolutionSpace();
        var removedCableSet = new RemovedCableSet(new List<Cable>());

        solutionSpace.EnqueueSolution(removedCableSet);

        while (true)
        {
            var currentSolution = solutionSpace.SolutionToCalculate.Dequeue();
            var graphView = new GraphView(immutableGraphTopology, currentSolution);
            if (!pathFinder.HasPath(graphView))
            {
                var juhu = true;
            }

            foreach (var stillUsableCable in graphView.GetAllStillUsableCables())
            {
                var possibleSolution = currentSolution.RemoveCable(stillUsableCable);
                if (solutionSpace.HasAlreadyEnumerated(possibleSolution)) continue;
                solutionSpace.EnqueueSolution(possibleSolution);
            }
        }

        //var priorityQueue = new PriorityQueue<SolutionCandidate, int>();
        //foreach (var availableEdge in availableEdges)
        //{
        //    if (CompareOrdinal(availableEdge.Key.from.Name, availableEdge.Key.to.Name) >= 0) continue;

        //    priorityQueue.Enqueue(new SolutionCandidate(availableEdge.Value, availableEdge.Key, availableEdges),
        //        availableEdge.Value);
        //}

        //while (priorityQueue.Count > 0)
        //{
        //    var solutionCandidate = priorityQueue.Dequeue();
        //    if (solutionCandidate.HasPath(pathFinder))
        //        foreach (var solution in solutionCandidate.EnumerateSolutions())
        //            priorityQueue.Enqueue(solution.Item1, solution.Item2);
        //}


        return "";
    }
}

internal class SolutionSpace
{
    private readonly HashSet<string> _alreadyEnumeratedSolutions = new();
    public PriorityQueue<RemovedCableSet, int> SolutionToCalculate { get; } = new();


    public bool HasAlreadyEnumerated(RemovedCableSet removedCableSet)
    {
        return _alreadyEnumeratedSolutions.Contains(removedCableSet.CanonicString);
    }

    public void EnqueueSolution(RemovedCableSet removedCableSet)
    {
        MarkAsAlreadyEnumerated(removedCableSet);
        SolutionToCalculate.Enqueue(removedCableSet, removedCableSet.Costs);
    }

    private void MarkAsAlreadyEnumerated(RemovedCableSet removedCableSet)
    {
        if (HasAlreadyEnumerated(removedCableSet))
            throw new InvalidOperationException("this solution has been considered already");

        _alreadyEnumeratedSolutions.Add(removedCableSet.CanonicString);
    }
}

internal class ImmutableGraphTopology
{
    private readonly Dictionary<Node, ReadOnlyCollection<(Node otherNode, Cable usedCable)>> _reachableNodesFrom;

    public ImmutableGraphTopology(ReadOnlyCollection<Cable> cables)
    {
        Cables = cables;

        var nodes = cables
            .Select(x => x.From)
            .Union(cables.Select(x => x.To))
            .Distinct()
            .Select(x => new Node(x))
            .ToList();

        StartNode = nodes.First(x => x.Name == "A");
        EndNode = nodes.First(x => x.Name == "Z");

        var dictionary = new Dictionary<Node, List<(Node otherNode, Cable usedCable)>>();
        foreach (var node in nodes) dictionary[node] = new List<(Node otherNode, Cable usedCable)>();

        foreach (var nodeFrom in nodes)
        foreach (var nodeTo in nodes)
        {
            var matchingCable = cables.FirstOrDefault(x =>
                (x.From == nodeFrom.Name && x.To == nodeTo.Name) ||
                (x.To == nodeFrom.Name && x.From == nodeTo.Name));

            if (matchingCable != null) dictionary[nodeFrom].Add((nodeTo, matchingCable));
        }

        _reachableNodesFrom = dictionary.ToDictionary(x => x.Key, x => x.Value.AsReadOnly());
    }

    public Node StartNode { get; }
    public Node EndNode { get; }

    public ReadOnlyCollection<Cable> Cables { get; }

    public ReadOnlyCollection<(Node otherNode, Cable usedCable)> GetAllConnectedNodes(Node fromNode)
    {
        return _reachableNodesFrom[fromNode];
    }
}

internal class RemovedCableSet
{
    private readonly IReadOnlyList<Cable> _removedCables;

    public RemovedCableSet(IReadOnlyList<Cable> removedCables)
    {
        _removedCables = removedCables;
        CanonicString = string.Join("-", removedCables.OrderBy(x => x.Id).Select(x => x.Id).AsEnumerable());
        Costs = removedCables.Sum(x => x.Thickness);
    }

    public string CanonicString { get; }
    public int Costs { get; }

    public bool CanUseCable(Cable cable)
    {
        return !_removedCables.Contains(cable);
    }

    public RemovedCableSet RemoveCable(Cable cable)
    {
        if (_removedCables.Contains(cable)) throw new InvalidOperationException("a cable can only be removed once");

        var removedCablesNew = _removedCables.ToList();
        removedCablesNew.Add(cable);
        return new RemovedCableSet(removedCablesNew);
    }
}

internal class GraphView
{
    private readonly RemovedCableSet _cableSet;
    private readonly ImmutableGraphTopology _immutableGraphTopology;

    public GraphView(ImmutableGraphTopology immutableGraphTopology, RemovedCableSet cableSet)
    {
        _immutableGraphTopology = immutableGraphTopology;
        _cableSet = cableSet;
    }

    public IEnumerable<(Node otherNode, Cable usedCable)> GetAllReachableNodesFrom(Node node)
    {
        return _immutableGraphTopology.GetAllConnectedNodes(node).Where(x => _cableSet.CanUseCable(x.usedCable));
    }

    public IEnumerable<Cable> GetAllStillUsableCables()
    {
        return _immutableGraphTopology.Cables.Where(x => _cableSet.CanUseCable(x));
    }
}

internal class PathFinding
{
    private readonly Node _endNodeGraph;
    private readonly Node _startNodeGraph;

    public PathFinding(Node startNodeGraph, Node endNodeGraph)
    {
        _startNodeGraph = startNodeGraph;
        _endNodeGraph = endNodeGraph;
    }

    public bool HasPath(GraphView graphView)
    {
        return HasPath(graphView, new HashSet<Node>(), _startNodeGraph);
    }

    private bool HasPath(GraphView graphView, HashSet<Node> visitedNodes,
        Node startNode)
    {
        if (startNode == _endNodeGraph) return true;


        foreach (var nextNodesToCheck in graphView.GetAllReachableNodesFrom(startNode)
                     .Where(x => !visitedNodes.Contains(x.otherNode)))
        {
            visitedNodes.Add(nextNodesToCheck.otherNode);
            if (HasPath(graphView, visitedNodes, nextNodesToCheck.otherNode)) return true;
            visitedNodes.Remove(nextNodesToCheck.otherNode);
        }

        return false;
    }
}

internal class Node
{
    public Node(string name)
    {
        Name = name;
    }

    public string Name { get; }
}